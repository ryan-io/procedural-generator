using System;
using System.Reflection;
using System.Threading;
using BCL;
using CommunityToolkit.HighPerformance;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using StateMachine;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	/// <summary>
	///     Verifies the scene contains the required components in order to run procedural generation logic
	///     This class will also kickoff the generation process
	/// 
	///     *** The Procedural Generator requires unsafe context ***
	/// 
	/// rowsOrHeight = GetLength(0)
	/// this is clearly opposite of what I thought
	/// https://stackoverflow.com/questions/4260207/how-do-you-get-the-width-and-height-of-a-multi-dimensional-array
	/// </summary>
	[HideMonoScript]
	public class ProceduralGenerator : Singleton<ProceduralGenerator, ProceduralGenerator>, ISeedInfo {
		[field: SerializeField, Required, BoxGroup("Configuration"), HideLabel]
		ProceduralConfig _config = null!;

		[field: SerializeField, Required, BoxGroup("Configuration"), HideLabel]
		SpriteShapeConfig _spriteShapeConfig = null!;

		[SerializeField, HideInInspector] MapData _data;

		public             ObservableCollection<string> Observables { get; private set; }
		public             TileHashset                  TileHashset { get; private set; }
		[CanBeNull] public MapData                      Data        => IsDataSet ? _data : default;

		bool IsRunning { get; set; }

		TileMapDictionary       TileMapDictionary       { get; set; }
		Grid                    Grid                    { get; set; }
		FillMapSolver           FillMapSolver           { get; set; }
		SmoothMapSolver         SmoothMapSolver         { get; set; }
		BorderAndBoundsSolver   BorderBoundsSolver      { get; set; }
		NodeSerializationSolver NodeSerializationSolver { get; set; }
		RegionRemovalSolver     RegionRemoverSolver     { get; set; }
		TileTypeSolver          TileTypeSolver          { get; set; }
		ErosionSolver           ErosionSolver           { get; set; }

		//TODO: this should be refactored to be more granular; it is too deep
		MeshSolver          MeshSolver          { get; set; }
		ColliderSolver      ColliderSolver      { get; set; }
		GridGraphBuilder    GridGraphBuilder    { get; set; }
		NavGraphRulesSolver NavGraphRulesSolver { get; set; }
		GraphScanner        GraphScanner        { get; set; }
		GeneratorSerializer GeneratorSerializer { get; set; }
		DataProcessor       DataProcessor       { get; set; }
		MeshRendering       Rendering           { get; set; }

		GameObject                GeneratedCollidersObj { get; set; }
		GeneratorTools            Tools                 { get; set; }
		StateMachine<ProcessStep> StateMachine          { get; set; }
		StopWatchWrapper          StopWatch             { get; set; }
		CancellationToken         CancellationToken     { get; set; }
		bool                      IsDataSet             { get; set; }

		public string CurrentSerializableName {
			get {
				var seedInfo = GetSeedInfo();
				return _config.Name         +
				       Constants.UNDERSCORE +
				       seedInfo.CurrentSeed +
				       Constants.UID        +
				       seedInfo.LastIteration;
			}
		}

		void Awake() {
			StartGeneration();
		}

		(TileMapDictionary dictionary, Grid grid) SetContainer() {
			var containerBuilder = new ContainerBuilder(gameObject);
			var output           = containerBuilder.Build();
			TileMapDictionary = output.dictionary;
			Grid              = output.grid;
			new SetGridPosition().Clean(_config, Grid);

			return output;
		}

		public SeedInfo GetSeedInfo() => new(_config.Seed, _config.LastSeed, _config.LastIteration);

		/// <summary>
		///     Starts the generation process. By default, will also invoke Initialize().
		/// </summary>
		/// <param name="alsoInitialize">If true, will invoke Initialize().</param>
		unsafe void StartGeneration(bool alsoInitialize = true) {
			Observables = new CreateObservables().Create(_config);
			StateMachine = new StateMachine<ProcessStep>(gameObject, true);
			
			if (IsRunning) {
				GenLogging.Instance.Log(Message.ALREADY_RUNNING, "Generation", LogLevel.Warning);
				return;
			}

			StateMachine.ChangeState(ProcessStep.Cleaning);
			Observables[StateObservableId.ON_CLEAN].Signal();
			CleanGenerator();
			//new EnsureCleanRootObject().Check(gameObject);

			IsRunning = true;

			if (_config.ShouldGenerate) {
				GeneratedCollidersObj = new ColliderGameObjectCreator().Create(this, CurrentSerializableName);
				var output = SetContainer();
			}
			if (alsoInitialize) {
				InitializeGenerator();
			}

			try {
#region SETUP

				if (!_config.ShouldGenerate) {
					if (_config.ShouldDeserialize)
						Deserialize();

					StateMachine.ChangeState(ProcessStep.Disposing);
					Observables[StateObservableId.ON_DISPOSE].Signal();
					return;
				}

#endregion

#region INITIALIZE

				StateMachine.ChangeState(ProcessStep.Initializing);
				Observables[StateObservableId.ON_INIT].Signal();

				var rowsOrHeight = _config.Rows;
				var colsOrWidth  = _config.Columns;
				var primaryPointer = stackalloc int[rowsOrHeight * colsOrWidth];
				var mapSpan        = new Span2D<int>(primaryPointer, rowsOrHeight, colsOrWidth, 0);

#endregion

#region RUN

				StateMachine.ChangeState(ProcessStep.Running);
				Observables[StateObservableId.ON_RUN].Signal();
				FillMapSolver.Fill(mapSpan);

				var array = mapSpan.ToArray();
				SmoothMapSolver.Smooth(array);
				mapSpan = new Span2D<int>(array);

				RegionRemoverSolver.Remove(mapSpan);

				//TODO: is this required?
				//var mapBorder = BorderBoundsSolver.DetermineBorderMap(mapSpan);

				TileTypeSolver.SetTiles(mapSpan);

				new TileMapCompressor(Grid.gameObject).Compress();

				var meshSolverData = MeshSolver.SolveAndCreate(mapSpan.ToArray());
				Rendering.Render(meshSolverData, Constants.SAVE_MESH_PREFIX + CurrentSerializableName);

				_data = new MapData(TileHashset, meshSolverData);

				var gridGraph = GridGraphBuilder.Build();

				NavGraphRulesSolver.ResetGridGraphRules(gridGraph);
				NavGraphRulesSolver.SetGridGraphRules(gridGraph);

				DataProcessor = new DataProcessor(_config, _data, TileMapDictionary, Grid, RegionRemoverSolver.Rooms);

				var erosionData = ErosionSolver.Erode(gridGraph);
				GraphScanner.ScanGraph(gridGraph);
				var boundaryCorners = ColliderSolver.Solve(_data, TileMapDictionary);

				GenLogging.Instance.Log("Setting shifted tile positions in map data", "MapData");

				//_data.TilePositionsShifted = erosionData.TilePositionsShifted;
				_data.BoundaryCorners = boundaryCorners;

				var borderSolver = new SpriteShapeBorderSolver(_spriteShapeConfig, gameObject);
				borderSolver.GenerateProceduralBorder(boundaryCorners, CurrentSerializableName);

				new CutGraphColliders().Cut(_config.ColliderCutters);
				new CreateBoundaryColliders(_config, DataProcessor).Create(GeneratedCollidersObj);
				new RenameTilemapContainer().Rename(CurrentSerializableName, Grid.gameObject);

				Tools.SetGridOrigin();
				Tools.SetGridScale(Constants.CELL_SIZE);
				
				if (_config.ShouldSerializeSeed)
					GeneratorSerializer.SerializeSeed(GetSeedInfo(), _config);

				if (_config.ShouldSerializePathfinding)
					GeneratorSerializer.SerializeCurrentAstarGraph(
						Constants.SAVE_ASTAR_PREFIX + CurrentSerializableName);

				if (_config.ShouldSerializeMapPrefab)
					GeneratorSerializer.SerializeMapGameObject(CurrentSerializableName, _config);

				if (_config.ShouldSerializeSpriteShape)
					GeneratorSerializer.SerializeSpriteShape(CurrentSerializableName, _data.GetSerializableBoundary());

#region COMPLETE

				new SetAllEdgeColliderRadius(_config.EdgeColliderRadius).Set(gameObject);
				StopWatch.Stop();
				StateMachine.ChangeState(ProcessStep.Completing);
				Observables[StateObservableId.ON_COMPLETE].Signal();

#endregion

#region DISPOSE

				// cleanup

				StateMachine.ChangeState(ProcessStep.Disposing);
				Observables[StateObservableId.ON_DISPOSE].Signal();
				StateMachine.DeleteSubscribers();

				GenLogging.Instance.LogWithTimeStamp(
					LogLevel.Normal,
					StopWatch.TimeElapsed,
					"Generation complete.",
					"Completion");

#endregion

#endregion

				// map of data goes here
			}
			catch (StackOverflowException e) {
#region STACKOVERFLOW

				GenLogging.Instance.LogWithTimeStamp(
					LogLevel.Error,
					StopWatch.TimeElapsed,
					e.Message,
					Message.STACK_OVERFLOW_ERROR);

				HandleErrorState();

#endregion
			}
			catch (Exception e) {
#region EXCEPTION

				GenLogging.Instance.LogWithTimeStamp(
					LogLevel.Error,
					StopWatch.TimeElapsed,
					e.Message + Constants.UNDERSCORE + e.Source,
					Message.CTX_ERROR + Constants.SPACE + e.TargetSite.Name + Constants.UNDERSCORE +
					e.GetMethodThatThrew(out var methodBase));

#endregion
			}

			finally {
				StateMachine.ChangeState(ProcessStep.Completing);
				Observables[StateObservableId.ON_COMPLETE].Signal();
				IsRunning = false;
			}
		}

		void Deserialize() {
			var deserializer = new GeneratorDeserializer(_config, StopWatch);

#region MAP_GAMEOBJECT

			var obj = deserializer.DeserializeMapPrefab(_config.NameSeedIteration);
			if (!obj) {
				GenLogging.Instance.Log("Could not find asset.", "DeserializeMap", LogLevel.Warning);
			}
			else {
				Instantiate(obj, gameObject.transform, true);
			}

#endregion

#region ASTAR_PATHFINDING

			deserializer.DeserializeAstar(_config.NameSeedIteration, _config.PathfindingSerializer);

#endregion

#region SPRITE_SHAPE_BOUNDARY

			// var positions = deserializer.DeserializeSpriteShape(_config.NameSeedIteration);
			// var solver    = new SpriteShapeBorderSolver(_spriteShapeConfig, gameObject);
			// solver.GenerateProceduralBorder(positions, _config.NameSeedIteration);

#endregion
		}

		void HandleErrorState() {
			StateMachine.ChangeState(ProcessStep.Error);
			Observables[StateObservableId.ON_ERROR].Signal();
		}
		
		void CleanGenerator(bool cleanRootObject = false) {
			try {
				if (cleanRootObject)
					new EnsureCleanRootObject().Check(gameObject);

#if UNITY_EDITOR
				var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
				var type     = assembly.GetType("UnityEditor.LogEntries");
				var method   = type.GetMethod("Clear");
				method?.Invoke(new object(), null);
#endif
				IsDataSet = false;
				GenLogging.Instance.ClearConsole();

				new ConfigCleaner().Clean(_config);
				new CleanSpriteShapes().Clean(gameObject);
				new ColliderGameObjectCleaner().Clean(gameObject, true);
				new MeshCleaner().Clean(gameObject);
				new GraphCleaner().Clean();
				new RenderCleaner().Clean(gameObject);
				new EnsureMapFitsOnStack().Ensure(_config);
				new DeallocateRoomMemory().Deallocate(RegionRemoverSolver);
				new CleanSpriteShapes().Clean(gameObject);
			}
			catch (Exception e) {
				GenLogging.Instance.LogWithTimeStamp(LogLevel.Error, 0f, e.Message, "CleanGenerator");
				throw;
			}
		}

		[BoxGroup("Actions", centerLabel: true),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void InitializeGenerator(bool isForced = false) {
			IsDataSet = false;

			if (_config.ShouldGenerate && !isForced)
				new SeedValidator(_config).Validate(this);

			new GetActiveAstarData().Retrieve();
			
			TileHashset        = new TileHashset();
			StopWatch          = new StopWatchWrapper(true);
			Tools              = new GeneratorTools(_config, Grid, StopWatch);
			FillMapSolver      = new CellularAutomataFillMapSolver(_config, StopWatch);
			SmoothMapSolver    = new MarchingSquaresSmoothMapSolver(_config, StopWatch);
			BorderBoundsSolver = new IterativeBorderAndBoundsSolver(_config, StopWatch);
			TileTypeSolver =
				new SetAllTilesSyncTileTypeSolver(_config, TileHashset, TileMapDictionary, Grid, StopWatch);
			NodeSerializationSolver = new NodeSerializationSolver(_config, this, TileMapDictionary, StopWatch);
			RegionRemoverSolver     = new FloodRegionRemovalSolver(_config);
			ErosionSolver           = new ErosionSolver(_config, TileHashset, TileMapDictionary);
			MeshSolver              = new MarchingSquaresMeshSolver(this);
			ColliderSolver          = new ColliderSolver(_config, gameObject, GeneratedCollidersObj, StopWatch);
			GridGraphBuilder        = new GridGraphBuilder(_config);
			NavGraphRulesSolver     = new NavGraphRulesSolver(TileMapDictionary);
			GraphScanner            = new GraphScanner(StopWatch);
			GeneratorSerializer     = new GeneratorSerializer(_config, Grid.gameObject, StopWatch);
			Rendering               = new MeshRendering(gameObject, default);
			CancellationToken       = this.GetCancellationTokenOnDestroy();
		}

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge),
		 ShowIf("@IsTileDictNullOrEmpty")]
		void PopulateTileDictionary() {
			if (_config.TileDictionary.Count < 1)
				_config.PopulateTileDictionary();
		}

		bool IsTileDictNullOrEmpty => _config.TileDictionary.IsEmptyOrNull();

		[BoxGroup("Actions", centerLabel: true),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void ForceClean() => CleanGenerator(true);
		
		[BoxGroup("Actions", centerLabel: true),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void ForceStop() {
			StateMachine?.ChangeState(ProcessStep.Completing);
			IsRunning = false;
		}

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void FindGraphCutters() => _config.FindGraphColliderCuttersInScene();

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void FindPathfinder() => _config.FindPathfinderInScene();

		[Button(ButtonSizes.Large, ButtonStyle.CompactBox, Icon = SdfIconType.Gear,
			IconAlignment = IconAlignment.RightOfText)]
		void Generate() {
			StartGeneration(true);
		}

		[Button]
		void TestGridBuild() {
			var containerBuilder = new ContainerBuilder(gameObject);
			var output           = containerBuilder.Build();
			TileMapDictionary = output.dictionary;
		}

		[Button]
		void LogDict() {
			if (TileMapDictionary == null)
				Debug.LogError("Dictionary is null");
			else {
				foreach (var pair in TileMapDictionary) {
					Debug.Log(pair.Value.GetType());
				}
			}
		}

		// void OnValidate() {
		// 	if (_config.ShouldGenerate) {
		// 		if (TileMapDictionary.IsEmptyOrNull() || !Grid) {
		// 			SetContainer();
		// 		}
		// 	}
		// }
	}
}