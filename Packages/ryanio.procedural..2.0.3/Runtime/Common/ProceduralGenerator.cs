using System;
using System.Collections.Generic;
using System.Linq;
using BCL;
using CommunityToolkit.HighPerformance;
using Sirenix.OdinInspector;
using StateMachine;
using TMPro;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///   This class is responsible for generating the map, mesh, collider, and serializing the data
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
	public class ProceduralGenerator : Singleton<ProceduralGenerator, ProceduralGenerator>, IOwner {
		// public string CurrentSerializableName {
		// 	get {
		// 		var seedInfo = GetSeedInfo();
		//
		// 		return _config.Name         +
		// 		       Constants.UNDERSCORE +
		// 		       seedInfo.Seed        +
		// 		       Constants.UID        +
		// 		       seedInfo.Iteration;
		// 	}
		// }

		public GameObject Go => gameObject;

		public ObservableCollection<string> Observables { get; private set; }
		public TileHashset                  TileHashset { get; private set; }

		bool IsRunning { get; set; }

		TileMapDictionary       TileMapDictionary       { get; set; }
		Grid                    Grid                    { get; set; }
		FillMapSolver           FillMapSolver           { get; set; }
		SmoothMapSolver         SmoothMapSolver         { get; set; }
		NodeSerializationSolver NodeSerializationSolver { get; set; }
		RegionRemovalSolver     RegionRemoverSolver     { get; set; }
		TileTypeSolver          TileSetterSolver          { get; set; }
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

		void Awake() {
			StartGeneration();
		}

		// (TileMapDictionary dictionary, Grid grid) SetContainer() {
		// 	var containerBuilder = new ContainerBuilder(gameObject);
		// 	var output           = containerBuilder.Build();
		// 	TileMapDictionary = output.dictionary;
		// 	Grid              = output.grid;
		// 	new SetGridPosition().Clean(_config, Grid);
		//
		// 	return output;
		// }

		//public SeedInfo GetSeedInfo() => new(_config.Seed, _config.LastIteration);

		/// <summary>
		///  Loads the generator. This is the entry point for the generator.
		///  Specify whether to generate or deserialize.
		///  If 'ShouldGenerate', will create a RunGenerator instance and invoke Run().
		///  else if 'ShouldDeserialize', will create a DeserializeGenerator instance and invoke Run().
		///  Otherwise, will do nothing.
		/// </summary>
		void Load() {
			var actions = new Actions(this)
				{ ProceduralConfig = _config, SpriteShapeConfig = _spriteShapeConfig };

			IMachine machine = GenerationMachine.Create(actions).Run();

			try {
				var onCompleteLog = string.Empty;

				if (!_config.ShouldGenerate && !_config.ShouldDeserialize) {
					actions.LogWarning(Message.NOT_SET_TO_RUN, nameof(Load));
					return;
				}

				new InitializationService(actions).Run(machine);
				var run = new Run(actions);

				if (_config.ShouldGenerate) {
					new SeedValidator(_config).Validate(actions.GetSeed());
					new GeneratorSetupService().Run();
					new GeneratorSceneSetupService(actions).Run();
					run.Generation(machine);
					run.Serialization(machine);

					onCompleteLog = Message.GENERATION_COMPLETE;
				}

				else if (_config.ShouldDeserialize) {
					new DeserializationService(actions).Run(actions);
					run.Deserialization(machine);

					onCompleteLog = Message.DESERIALIZATION_COMPLETE;
				}
                
				// dispose
				machine.InvokeEvent(StateObservableId.ON_DISPOSE);
				
				// complete
				actions.StopTimer();
				machine.InvokeEvent(StateObservableId.ON_COMPLETE);
				actions.Log(onCompleteLog, nameof(Load));
			}
			catch (Exception e) {
				machine.InvokeEvent(StateObservableId.ON_ERROR);
				actions.LogError(e.Message, nameof(Load));
			}
		}

		/// <summary>
		///     Starts the generation process. By default, will also invoke Initialize().
		/// </summary>
		/// <param name="alsoInitialize">If true, will invoke Initialize().</param>
		unsafe void StartGeneration(bool alsoInitialize = true) {
			//CleanGenerator(); //DONE

			// if (_config.ShouldGenerate) {
			// 	//GeneratedCollidersObj = new ColliderGameObjectCreator().Create(this);
			// 	//var output = SetContainer();
			// }

			// if (alsoInitialize) {
			// 	if (_config.ShouldDeserialize && !_config.ShouldGenerate)
			// 		InitializeGenerator(true);
			// 	else InitializeGenerator();
			// }

			try {
				// if (!_config.ShouldGenerate && _config.ShouldDeserialize) {
				// 	//GeneratedCollidersObj = new ColliderGameObjectCreator().Create(this);
				// 	var deserializeRouter =
				// 		new DeserializationRouter(gameObject, _config, _spriteShapeConfig, StopWatch);
				// 	deserializeRouter.ValidateAndDeserialize(_config.NameSeedIteration, GeneratedCollidersObj);
				//
				// 	StateMachine.ChangeState(ProcessStep.Disposing);
				// 	Observables[StateObservableId.ON_DISPOSE].Signal();
				// 	return;
				// }


				//
				// StateMachine.ChangeState(ProcessStep.Initializing);
				// Observables[StateObservableId.ON_INIT].Signal();
				//
				// var rowsOrHeight   = _config.Rows;
				// var colsOrWidth    = _config.Columns;
				// var primaryPointer = stackalloc int[rowsOrHeight * colsOrWidth];
				// var mapSpan        = new Span2D<int>(primaryPointer, rowsOrHeight, colsOrWidth, 0);


#region RUN

				// StateMachine.ChangeState(ProcessStep.Running);
				// Observables[StateObservableId.ON_RUN].Signal();
				//FillMapSolver.Fill(mapSpan);

				// var array = mapSpan.ToArray();
				// SmoothMapSolver.Smooth(array);
				// mapSpan = new Span2D<int>(array);

				//RegionRemoverSolver.Remove(mapSpan);

				//TileSetterSolver.Set(mapSpan);

				//new TileMapCompressor(Grid.gameObject).Compress();

				//var meshSolverData = MeshSolver.Create(mapSpan.ToArray());
				//Rendering.Render(meshSolverData, Constants.SAVE_MESH_PREFIX + CurrentSerializableName);

				//_data = new MapData(TileHashset, meshSolverData);

				// var gridGraph = GridGraphBuilder.Build();
				//
				// NavGraphRulesSolver.ResetGridGraphRules(gridGraph);
				// NavGraphRulesSolver.SetGridGraphRules(gridGraph);

				//???
				DataProcessor = new DataProcessor(_config, _data, TileMapDictionary, Grid, RegionRemoverSolver.Rooms);

				//var erosionData = ErosionSolver.Erode(gridGraph);
				//GraphScanner.ScanGraph(gridGraph);
				// Dictionary<int, List<Vector3>> dict;
				//  var                            colliderCoords = new Dictionary<int, List<SerializableVector3>>();
				// (_data.SpriteBoundaryCoords, dict) = ColliderSolver.Solve(_data, TileMapDictionary);
				//
				// for (var i = 0; i < dict.Count; i++) {
				// 	colliderCoords[i] = dict[i].AsSerialized().ToList();
				// }

				//GenLogging.Instance.Log("Setting shifted tile positions in map data", "MapData");

				// var borderSolver = new SpriteShapeBorderSolver(_spriteShapeConfig, gameObject);
				// borderSolver.Generate(_data.SpriteBoundaryCoords, CurrentSerializableName);

				//new CutGraphColliders().Cut(_config.ColliderCutters);		moveted to NavigationSolver
				//new CreateBoundaryColliders(_config, DataProcessor).Set(GeneratedCollidersObj);
				// new RenameTilemapContainer().Rename(CurrentSerializableName, Grid.gameObject);
				//
				// Tools.SetOriginWrtMap(Grid.gameObject);
				// Tools.SetGridScale(Constants.CELL_SIZE);

				//GeneratorSerializer.SerializeSeed(GetSeedInfo(), _config);

				// new SetColliderObjName().Set(GeneratedCollidersObj,
				// 	Constants.SAVE_COLLIDERS_PREFIX + CurrentSerializableName);

#region COMPLETE

				//new SetAllEdgeColliderRadius(_config.EdgeColliderRadius).Set(gameObject);
				// var router = new SerializationRouter(_config, Grid.gameObject, StopWatch);
				// router.Run(CurrentSerializableName, directory, _data.GetBoundaryCoords(),
				// 	colliderCoords);
				//StopWatch.Stop();
				// StateMachine.ChangeState(ProcessStep.Completing);
				// Observables[StateObservableId.ON_COMPLETE].Signal();

#endregion

#region DISPOSE

				// cleanup

				StateMachine.ChangeState(ProcessStep.Disposing);
				Observables[StateObservableId.ON_DISPOSE].Signal();
				StateMachine.DeleteSubscribers();

				// GenLogging.Instance.LogWithTimeStamp(
				// 	LogLevel.Normal,
				// 	StopWatch.TimeElapsed,
				// 	"Generation complete.",
				// 	"Completion");

#endregion

#endregion

				// map of data goes here
			}
			catch (StackOverflowException e) {
#region STACKOVERFLOW

				// GenLogging.Instance.LogWithTimeStamp(
				// 	LogLevel.Error,
				// 	StopWatch.TimeElapsed,
				// 	e.Message,
				// 	Message.STACK_OVERFLOW_ERROR);

				HandleErrorState();

#endregion
			}
			catch (Exception e) {
#region EXCEPTION

				// GenLogging.Instance.LogWithTimeStamp(
				// 	LogLevel.Error,
				// 	StopWatch.TimeElapsed,
				// 	e.Message + Constants.UNDERSCORE + e.Source,
				// 	Message.CTX_ERROR + Constants.SPACE + e.TargetSite.Name + Constants.UNDERSCORE +
				// 	e.GetMethodThatThrew(out var methodBase));

#endregion
			}

			finally {
				StateMachine.ChangeState(ProcessStep.Completing);
				Observables[StateObservableId.ON_COMPLETE].Signal();
				IsRunning = false;
			}
		}

		void HandleErrorState() {
			StateMachine.ChangeState(ProcessStep.Error);
			Observables[StateObservableId.ON_ERROR].Signal();
		}

		// void CleanGenerator(bool cleanRootObject = false) {
		// 		// new ConfigCleaner().Clean(_config);
		// 		// new CleanSpriteShapes().Clean(gameObject);
		// 		// new ColliderGameObjectCleaner().Clean(gameObject, true);
		// 		// new MeshCleaner().Clean(gameObject);
		// 		// new GraphCleaner().Clean();
		// 		// new RenderCleaner().Clean(gameObject);
		// 		// new EnsureMapFitsOnStack().Ensure(_config);
		// 		// new DeallocateRoomMemory().Deallocate(RegionRemoverSolver);
		// 		// new CleanSpriteShapes().Clean(gameObject);
		// }

		void InitializeGenerator(bool isForced = false) {
			// if (_config.ShouldGenerate && !isForced)
			// 	new SeedValidator(_config).Validate(this);

			//new ActiveAstarData().Retrieve();

			if (_config.ShouldDeserialize)
				return;

			// TileHashset     = new TileHashset();
			// StopWatch       = new StopWatchWrapper(true);
			// Tools           = new GeneratorTools(_config, Grid, StopWatch);
			// FillMapSolver   = new CellularAutomataFillMapSolver(_config, StopWatch);
			// SmoothMapSolver = new MarchingSquaresSmoothMapSolver(_config, StopWatch);
			// TileTypeSolver =
			// 	new SetAllTilesSyncTileTypeSolver(_config, TileHashset, TileMapDictionary, Grid, StopWatch);
			// NodeSerializationSolver = new NodeSerializationSolver(_config, this, TileMapDictionary, StopWatch);
			// RegionRemoverSolver     = new FloodRegionRemovalSolver(_config);
			// ErosionSolver           = new ErosionSolver(_config, TileHashset, TileMapDictionary);
			// MeshSolver              = new MarchingSquaresMeshSolver(this);
			// ColliderSolver          = new ColliderSolver(_config, gameObject, GeneratedCollidersObj, StopWatch);
			// GridGraphBuilder        = new GridGraphBuilder(_config);
			// NavGraphRulesSolver     = new NavGraphRulesSolver(TileMapDictionary);
			// GraphScanner            = new GraphScanner(StopWatch);
			// GeneratorSerializer     = new GeneratorSerializer(_config, Grid.gameObject, StopWatch);
			// Rendering               = new MeshRendering(gameObject, default);
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
		void ForceClean() => new GeneratorCleaner(new Actions(this)).Clean();

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

		[BoxGroup("Actions"),
		 HorizontalGroup("Actions/Buttons2"),
		 ButtonGroup("Actions/Buttons2/Methods", Stretch = false, IconAlignment = IconAlignment.RightEdge)]
		void DeleteSelectedSerialized() => DirectoryAction.DeleteDirectory(_config.NameSeedIteration);

		[Button(ButtonSizes.Medium, ButtonStyle.CompactBox, Icon = SdfIconType.Signal,
			IconAlignment = IconAlignment.RightOfText)]
		void ScaleMap(float scale) => Scale.Current(gameObject, scale);

		[Button(ButtonSizes.Large, ButtonStyle.CompactBox, Icon = SdfIconType.Gear,
			IconAlignment = IconAlignment.RightOfText)]
		void Generate() {
			StartGeneration(true);
		}

		[field: SerializeField, Required, BoxGroup("Configuration"), HideLabel]
		ProceduralConfig _config = null!;

		[field: SerializeField, Required, BoxGroup("Configuration"), HideLabel]
		SpriteShapeConfig _spriteShapeConfig = null!;

		[SerializeField, HideInInspector] MapData _data;
	}
}