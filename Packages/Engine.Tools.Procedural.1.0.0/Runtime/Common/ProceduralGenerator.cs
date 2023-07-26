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

		FillMapSolver           FillMapSolver           { get; set; }
		SmoothMapSolver         SmoothMapSolver         { get; set; }
		BorderAndBoundsSolver   BorderBoundsSolver      { get; set; }
		NodeSerializationSolver NodeSerializationSolver { get; set; }
		RegionRemovalSolver     RegionRemoverSolver     { get; set; }
		TileTypeSolver          TileTypeSolver          { get; set; }
		ErosionSolver           ErosionSolver           { get; set; }

		//TODO: this should be refactored to be more granular; it is too deep
		MeshSolver           MeshSolver           { get; set; }
		ColliderSolver       ColliderSolver       { get; set; }
		GridGraphBuilder     GridGraphBuilder     { get; set; }
		NavGraphRulesSolver  NavGraphRulesSolver  { get; set; }
		GraphScanner         GraphScanner         { get; set; }
		ProceduralSerializer ProceduralSerializer { get; set; }
		DataProcessor        DataProcessor        { get; set; }
		MeshRendering        Rendering            { get; set; }

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
			InitializeGenerator();
		}

		public SeedInfo GetSeedInfo() => new(_config.Seed, _config.LastSeed, _config.LastIteration);

		/// <summary>
		///     Starts the generation process. By default, will also invoke Initialize().
		/// </summary>
		/// <param name="alsoInitialize">If true, will invoke Initialize().</param>
		unsafe void StartGeneration(bool alsoInitialize = true) {
			try {
#region CLEAN

				if (!_config.ShouldGenerate)
					// do not proceed any further; redirect control elsewhere
					return;

				if (alsoInitialize) {
					InitializeGenerator();
				}

				StateMachine.ChangeState(ProcessStep.Cleaning);
				Observables[StateObservableId.ON_CLEAN].Signal();
				CleanGenerator();

#endregion

#region INITIALIZE

				StateMachine.ChangeState(ProcessStep.Initializing);
				Observables[StateObservableId.ON_INIT].Signal();

				Tools.SetGridOrigin();
				Tools.SetGridScale(Constants.CELL_SIZE);

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

				new TileMapCompressor(_config.TilemapContainer).Compress();

				var meshSolverData = MeshSolver.SolveAndCreate(mapSpan.ToArray());
				Rendering.Render(meshSolverData, Constants.SAVE_MESH_PREFIX + CurrentSerializableName);

				_data = new MapData(TileHashset, meshSolverData);

				var gridGraph = GridGraphBuilder.Build();

				NavGraphRulesSolver.ResetGridGraphRules(gridGraph);
				NavGraphRulesSolver.SetGridGraphRules(gridGraph);

				DataProcessor = new DataProcessor(_config, _data, RegionRemoverSolver.Rooms);

				var erosionData = ErosionSolver.Erode(gridGraph);
				GraphScanner.ScanGraph(gridGraph);
				ColliderSolver.Solve(_data);

				GenLogging.Instance.Log("Setting shifted tile positions in map data", "MapData");

				//_data.TilePositionsShifted = erosionData.TilePositionsShifted;

				// var borderSolver = new SpriteShapeBorderSolver(_spriteShapeConfig, CurrentSerializableName);
				// borderSolver.GenerateProceduralBorder(_data, gameObject);

				new CutGraphColliders().Cut(_config.ColliderCutters);
				new CreateBoundaryColliders(_config, DataProcessor).Create(GeneratedCollidersObj);

				//DataProcessor.IsReady = true;

				if (_config.ShouldSerializeSeed)
					ProceduralSerializer.SerializeSeed(GetSeedInfo(), _config);

				if (_config.ShouldSerializePathfinding)
					ProceduralSerializer.SerializeCurrentAstarGraph(
						Constants.SAVE_ASTAR_PREFIX + CurrentSerializableName);

				if (_config.ShouldSerializeMapPrefab)
					ProceduralSerializer.SerializeMapGameObject(CurrentSerializableName, _config);

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
		}

		void HandleErrorState() {
			StateMachine.ChangeState(ProcessStep.Error);
			Observables[StateObservableId.ON_ERROR].Signal();
		}

		void HandleGeneratorDidNotRun() {
			ProceduralSerializer.DeserializeAstarGraph(_config.NameSeedIteration, _config);
		}

		[BoxGroup("Actions", centerLabel: true),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void CleanGenerator() {
			try {
#if UNITY_EDITOR
				var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
				var type = assembly.GetType("UnityEditor.LogEntries");
				var method = type.GetMethod("Clear");
				method?.Invoke(new object(), null);
#endif
				IsDataSet = false;
				GenLogging.Instance.ClearConsole();

				new ConfigCleaner().Clean(_config);
				new TilemapCleaner().Clean(_config);
				new ColliderGameObjectCleaner().Clean(gameObject, true);
				new MeshCleaner().Clean(gameObject);
				new GridCleaner().Clean(_config);
				new GraphCleaner().Clean();
				new RenderCleaner().Clean(gameObject);
				new EnsureMapFitsOnStack().Ensure(_config);
				new DeallocateRoomMemory().Deallocate(RegionRemoverSolver);
			}
			catch (Exception e) {
				GenLogging.Instance.LogWithTimeStamp(LogLevel.Error, 0f, e.Message, "CleanGenerator");
				throw;
			}
		}

		[BoxGroup("Actions", centerLabel: true),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void InitializeGenerator() {
			IsDataSet = false;
			new SeedValidator(_config).Validate();
			new GetActiveAstarData().Retrieve();

			StateMachine            = new StateMachine<ProcessStep>(gameObject, true);
			TileHashset             = new TileHashset();
			StopWatch               = new StopWatchWrapper(true);
			Tools                   = new GeneratorTools(_config, StopWatch);
			FillMapSolver           = new CellularAutomataFillMapSolver(_config, StopWatch);
			SmoothMapSolver         = new MarchingSquaresSmoothMapSolver(_config, StopWatch);
			BorderBoundsSolver      = new IterativeBorderAndBoundsSolver(_config, StopWatch);
			TileTypeSolver          = new SetAllTilesSyncTileTypeSolver(_config, TileHashset, StopWatch);
			NodeSerializationSolver = new NodeSerializationSolver(_config, this, StopWatch);
			RegionRemoverSolver     = new FloodRegionRemovalSolver(_config);
			ErosionSolver           = new ErosionSolver(_config, TileHashset);
			GeneratedCollidersObj   = new ColliderGameObjectCreator().Create(this, CurrentSerializableName);
			MeshSolver              = new MarchingSquaresMeshSolver(this);
			ColliderSolver          = new ColliderSolver(_config, gameObject, GeneratedCollidersObj, StopWatch);
			GridGraphBuilder        = new GridGraphBuilder(_config);
			NavGraphRulesSolver     = new NavGraphRulesSolver(_config);
			GraphScanner            = new GraphScanner(StopWatch);
			ProceduralSerializer    = new ProceduralSerializer(_config, StopWatch);
			Rendering               = new MeshRendering(gameObject, default);

			Observables = new ObservableCollection<string> {
				{
					StateObservableId.ON_CLEAN, new Observable(
						_config.SerializedEvents[ProcessStep.Cleaning].Invoke)
				}, {
					StateObservableId.ON_INIT, new Observable(
						_config.SerializedEvents[ProcessStep.Initializing].Invoke)
				}, {
					StateObservableId.ON_RUN, new Observable(
						_config.SerializedEvents[ProcessStep.Running].Invoke)
				}, {
					StateObservableId.ON_COMPLETE, new Observable(
						_config.SerializedEvents[ProcessStep.Completing].Invoke)
				}, {
					StateObservableId.ON_DISPOSE, new Observable(
						_config.SerializedEvents[ProcessStep.Disposing].Invoke)
				}, {
					StateObservableId.ON_ERROR, new Observable(
						_config.SerializedEvents[ProcessStep.Error].Invoke)
				}
			};

			CancellationToken = this.GetCancellationTokenOnDestroy();
		}

		void OnDrawGizmosSelected() {
			if (ColliderSolver == null || ColliderSolver.ProcessedBorderPositions.IsEmptyOrNull()) return;
			
			foreach (var vector in ColliderSolver.ProcessedBorderPositions) {
				DebugExt.DrawCircle(vector, Color.white, true,  .5f);
				DebugExt.DrawPoint(vector, Color.magenta,   1f);
			}

			if (DataProcessor == null || !_config.DrawDebugGizmos || !DataProcessor.IsReady) return;

			//RoomCalculator.DrawRooms();
			// DataProcessor.DrawMapBoundary();
			//DataProcessor.DrawRoomOutlines();
			//DataProcessor.DrawTilePositionsShifted();
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

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void FindGraphCutters() => _config.FindGraphColliderCuttersInScene();

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons1"),
		 ButtonGroup("Actions/Buttons1/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void FindPathfinder() => _config.FindPathfinderInScene();

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void DeserializeAstar() {
			ProceduralSerializer.DeserializeAstarGraph(CurrentSerializableName, _config);
		}

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons3"),
		 ButtonGroup("Actions/Buttons3/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge),
		 PropertySpace(75f, 75f)]
		void Generate() {
			StartGeneration(true);
		}
	}
}