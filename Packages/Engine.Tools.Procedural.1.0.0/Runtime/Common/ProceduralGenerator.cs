using System;
using System.Diagnostics;
using System.Linq;
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
using Debug = UnityEngine.Debug;

namespace Engine.Procedural {
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
	[RequireComponent(typeof(MeshFilter))]
	public class ProceduralGenerator : Singleton<ProceduralGenerator, ProceduralGenerator>, ISeedInfo {
		[field: SerializeField] [field: Required]
		ProceduralConfig _config = null!;

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

		GameObject                GeneratedCollidersObj { get; set; }
		GameObject                PathfindingMeshObj    { get; set; }
		GeneratorTools            Tools                 { get; set; }
		StateMachine<ProcessStep> StateMachine          { get; set; }
		StopWatchWrapper          StopWatch             { get; set; }
		CancellationToken         CancellationToken     { get; set; }
		bool                      IsDataSet             { get; set; }

		string GetAstarSerializationName {
			get {
				var seedInfo = GetSeedInfo();

				return Constants.SAVE_ASTAR_PREFIX +
				       _config.Name                +
				       Constants.UNDERSCORE        +
				       seedInfo.CurrentSeed        +
				       Constants.UID               +
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
		[Button]
		unsafe void StartGeneration(bool alsoInitialize = true) {
			try {
#region CLEAN

				StateMachine.ChangeState(ProcessStep.Cleaning);
				Observables[StateObservableId.ON_CLEAN].Signal();
				CleanGenerator();

#endregion

#region INITIALIZE

				StateMachine.ChangeState(ProcessStep.Initializing);
				Observables[StateObservableId.ON_INIT].Signal();

				Tools.SetGridOrigin();
				Tools.SetGridScale(Constants.CELL_SIZE);

				if (!_config.ShouldGenerate)
					// do not proceed any further; redirect control elsewhere
					return;

				if (alsoInitialize) {
					InitializeGenerator();
				}

				var rowsOrHeight = _config.Rows;
				var colsOrWidth  = _config.Columns;

				var primaryPointer = stackalloc int[rowsOrHeight * colsOrWidth];
				var mapSpan        = new Span2D<int>(primaryPointer, rowsOrHeight, colsOrWidth, 0);

#endregion

#region RUN

				StateMachine.ChangeState(ProcessStep.Running);
				Observables[StateObservableId.ON_RUN].Signal();

				FillMapSolver.Fill(mapSpan);
				SmoothMapSolver.Smooth(mapSpan);
				RegionRemoverSolver.Remove(mapSpan);

				var mapBorder = BorderBoundsSolver.DetermineBorderMap(mapSpan);

				TileTypeSolver.SetTiles(mapSpan);

				new TileMapCompressor(_config.TilemapContainer).Compress();

				//TODO: The reset of the  generation will utilize the previous array code instead of span
				var meshSolverData = MeshSolver.SolveAndCreate(mapBorder);

				_data = new MapData(TileHashset, meshSolverData);

				new PrepPathfindingMesh(gameObject).Prep(PathfindingMeshObj, meshSolverData);
				var gridGraph = GridGraphBuilder.Build();

				NavGraphRulesSolver.ResetGridGraphRules(gridGraph);
				NavGraphRulesSolver.SetGridGraphRules(gridGraph);

				DataProcessor = new DataProcessor(_config, _data, RegionRemoverSolver.Rooms);

// 				var scannerArgs = new GraphScanner.Args(
// 					gridGraph, 
// 					() => {
// 						//var erosionData = ErosionSolver.Erode(gridGraph);
// 						ColliderSolver.Solve(_data);
// 					
// 						GenLogging.Instance.Log("Setting shifted tile positions in map data", "MapData");
// 					
// 						//_data.TilePositionsShifted = erosionData.TilePositionsShifted;
// 					
// 						new CutGraphColliders().Cut(_config.ColliderCutters);
// 						new SerializeSeedInfo().Serialize(GetSeedInfo(), _config.SeedInfoSerializer, _config.Name, StopWatch);
// 						new CreateBoundaryColliders(_config, DataProcessor).Create(GeneratedCollidersObj);
// 					
// 						//DataProcessor.IsReady = true;
// 					
// 						AstarSerializer.SerializeCurrentAstarGraph(
// 							_config.PathfindingSerializer,
// 							GetAstarSerializationName);
// 						
// #region COMPLETE
//
// 						new SetAllEdgeColliderRadius(_config.EdgeColliderRadius).Set(gameObject);
// 						StopWatch.Stop();
// 						StateMachine.ChangeState(ProcessStep.Completing);
// 						Observables[StateObservableId.ON_COMPLETE].Signal();
//
// #endregion
//
// #region DISPOSE
//
// 						// cleanup
//
// 						StateMachine.ChangeState(ProcessStep.Disposing);
// 						Observables[StateObservableId.ON_DISPOSE].Signal();
// 						StateMachine.DeleteSubscribers();
//
// 						GenLogging.Instance.LogWithTimeStamp(
// 							LogLevel.Normal,
// 							StopWatch.TimeElapsed,
// 							"Generation complete.",
// 							"Completion");
//
// #endregion
// 					},
// 					true);
// 				
				//GraphScanner.FireForget(scannerArgs, CancellationToken);

				var erosionData = ErosionSolver.Erode(gridGraph);
				GraphScanner.ScanGraph(gridGraph);
				ColliderSolver.Solve(_data);

				GenLogging.Instance.Log("Setting shifted tile positions in map data", "MapData");

				//_data.TilePositionsShifted = erosionData.TilePositionsShifted;

				new CutGraphColliders().Cut(_config.ColliderCutters);
				new SerializeSeedInfo().Serialize(GetSeedInfo(), _config.SeedInfoSerializer, _config.Name, StopWatch);
				new CreateBoundaryColliders(_config, DataProcessor).Create(GeneratedCollidersObj);

				//DataProcessor.IsReady = true;

				ProceduralSerializer.SerializeCurrentAstarGraph(
					_config.PathfindingSerializer,
					GetAstarSerializationName);

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
#region EXCEPTIONS

				GenLogging.Instance.LogWithTimeStamp(
					LogLevel.Error,
					StopWatch.TimeElapsed,
					e.Message + Constants.UNDERSCORE + e.Source,
					Message.CTX_ERROR + Constants.SPACE + e.TargetSite.Name + Constants.UNDERSCORE +
					e.GetMethodThatThrew(out _));

#endregion
			}
		}

		void HandleErrorState() {
			StateMachine.ChangeState(ProcessStep.Error);
			Observables[StateObservableId.ON_ERROR].Signal();
		}

		[Button]
		void CleanGenerator() {
			try {
				IsDataSet = false;
				GenLogging.Instance.ClearConsole();

				new ConfigCleaner().Clean(_config);
				new TilemapCleaner().Clean(_config);
				new ColliderGameObjectCleaner().Clean(gameObject);
				new MeshCleaner().Clean(gameObject);
				new GridCleaner().Clean(_config);
				new GraphCleaner().Clean();
				new EnsureMapFitsOnStack().Ensure(_config);
				new DeallocateRoomMemory().Deallocate(RegionRemoverSolver);
			}
			catch (Exception e) {
				GenLogging.Instance.LogWithTimeStamp(LogLevel.Error, 0f, e.Message, "CleanGenerator");
				throw;
			}
		}

		void HandleGeneratorDidNotRun() {
			// deserialize Astar data and try to parse serialized data to a pathfinding gridGraph
			ProceduralSerializer.DeserializeAstarGraph(_config);
			
			//await NodeSerializationSolver.FireTask(CancellationToken);
		}

		public readonly struct TilemapSetup {
			public void Setup(ProceduralConfig config) {
				
			}

		}

		[Button]
		void InitializeGenerator() {
			IsDataSet = false;
			new SeedValidator(_config).Validate();
			new TilemapSetup().Setup(_config);
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
			GeneratedCollidersObj   = new EdgeColliderCreator().Create(this);
			MeshSolver              = new MarchingSquaresMeshSolver(_config);
			ColliderSolver          = new ColliderSolver(_config, gameObject, GeneratedCollidersObj, StopWatch);
			GridGraphBuilder        = new GridGraphBuilder(_config);
			NavGraphRulesSolver     = new NavGraphRulesSolver(_config);
			GraphScanner            = new GraphScanner(StopWatch);
			ProceduralSerializer    = new ProceduralSerializer(_config, StopWatch);

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
			if (DataProcessor == null || !_config.DrawDebugGizmos || !DataProcessor.IsReady) return;

			//RoomCalculator.DrawRooms();
			// DataProcessor.DrawMapBoundary();
			//DataProcessor.DrawRoomOutlines();
			//DataProcessor.DrawTilePositionsShifted();
		}

		[Button]
		void ValidateSerializedFiles() {
			ProceduralSerializer.DeserializeAstarGraph(_config);
		}
	}
}