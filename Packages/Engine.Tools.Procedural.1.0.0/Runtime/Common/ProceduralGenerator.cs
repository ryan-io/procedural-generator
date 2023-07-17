using System;
using System.Diagnostics;
using System.Threading;
using BCL;
using CommunityToolkit.HighPerformance;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using StateMachine;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	/// <summary>
	///     Verifies the scene contains the required components in order to run procedural generation logic
	///     This class will also kickoff the generation process
	///     *** The Procedural Generator requires unsafe context ***
	/// </summary>
	[RequireComponent(typeof(MeshFilter))]
	public class ProceduralGenerator : Singleton<ProceduralGenerator, ProceduralGenerator>,
	                                   ISeedInfo {
		[field: SerializeField] [field: Required]
		ProceduralConfig _config = null!;

		public ObservableCollection<string> Observables { get; private set; }
		public TileHashset                  TileHashset { get; private set; }

		FillMapSolver           FillMapSolver           { get; set; }
		SmoothMapSolver         SmoothMapSolver         { get; set; }
		BorderAndBoundsSolver   BorderBoundsSolver      { get; set; }
		NodeSerializationSolver NodeSerializationSolver { get; set; }
		RegionRemovalSolver     RegionRemoverSolver     { get; set; }
		TileTypeSolver          TileTypeSolver          { get; set; }
		ErosionSolver           ErosionSolver           { get; set; }

		//TODO: this should be refactored to be more granular; it is too deep
		MeshAndColliderSolver MeshAndColliderSolver { get; set; }
		GridGraphBuilder      GridGraphBuilder      { get; set; }
		NavGraphRulesSolver   NavGraphRulesSolver   { get; set; }
		GraphScanner          GraphScanner          { get; set; }
		AstarSerializer       AstarSerializer       { get; set; }

		GameObject                GeneratedCollidersObj { get; set; }
		GameObject                PathfindingMeshObj    { get; set; }
		GeneratorTools            Tools                 { get; set; }
		StateMachine<ProcessStep> StateMachine          { get; set; }
		StopWatchWrapper          StopWatch             { get; set; }
		CancellationToken         CancellationToken     { get; set; }

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

				if (!_config.ShouldGenerate)
					// do not proceed any further; redirect control elsewhere
					return;

				if (alsoInitialize) {
					InitializeGenerator();
				}

				// rowsOrHeight = GetLength(0)
				// colsOrWidth = GetLength(1)
				// this is clearly opposite of what I thought
				// https://stackoverflow.com/questions/4260207/how-do-you-get-the-width-and-height-of-a-multi-dimensional-array
				var rowsOrHeight = _config.Width;
				var colsOrWidth  = _config.Height;
				
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

				Tools.SetGridOrigin();
				Tools.SetGridScale(Constants.CELL_SIZE);

				//TODO: The reset of the  generation will utilize the previous array code instead of span
				var meshData = MeshAndColliderSolver.SolveAndCreate(mapBorder);
				//var meshData = MeshTriangulationSolver.Triangulate(mapBorder);
				new PrepPathfindingMesh(gameObject).Prep( PathfindingMeshObj, meshData);
				var gridGraph = GridGraphBuilder.Build();
				NavGraphRulesSolver.ResetGridGraphRules(gridGraph);
				NavGraphRulesSolver.SetGridGraphRules(gridGraph);
				ErosionSolver.Erode(gridGraph);

				// walkability solver logic goes here

				var scannerArgs = new GraphScanner.Args(
					gridGraph,
					() => {
						AstarSerializer.SerializeCurrentAstarGraph(
							_config.PathfindingSerializer,
							GetAstarSerializationName);
					},
					true);

				GraphScanner.FireForget(scannerArgs, CancellationToken);
				new CutGraphColliders().Cut(_config.ColliderCutters);

				//TODO: solver(s) pertaining to rooms go here (WIP)
				/*
				 * _roomSolver = new SimpleRoomSolver();
				 * await _roomSolver.Solve();
				 */

				new SerializeSeedInfo().Serialize(GetSeedInfo(), _config.SeedInfoSerializer, _config.Name, StopWatch);

#endregion

				// map of data goes here
			}
			catch (StackOverflowException e) {
				GenLogging.LogWithTimeStamp(
					LogLevel.Error,
					StopWatch.TimeElapsed,
					e.Message,
					Message.STACK_OVERFLOW_ERROR);

				HandleErrorState();
			}

			catch (Exception e) {
				var stackTrace = new StackTrace(e).GetFrame(0).GetMethod().Name;

				GenLogging.LogWithTimeStamp(
					LogLevel.Error,
					StopWatch.TimeElapsed,
					e.Message,
					Message.CTX_ERROR + Constants.SPACE + e.TargetSite.Name + Constants.UNDERSCORE + stackTrace);

				HandleErrorState();
			}

#region COMPLETE

			finally {
				new SetAllEdgeColliderRadius(_config.EdgeColliderRadius).Set(gameObject);
				StopWatch.Stop();
				StateMachine.ChangeState(ProcessStep.Completing);
				Observables[StateObservableId.ON_COMPLETE].Signal();
				Tools.LogHeapMemoryAllocated();

#endregion

#region DISPOSE

				// cleanup

				StateMachine.ChangeState(ProcessStep.Disposing);
				Observables[StateObservableId.ON_DISPOSE].Signal();
				StateMachine.DeleteSubscribers();

#endregion
			}
		}

		void HandleErrorState() {
			StateMachine.ChangeState(ProcessStep.Error);
			Observables[StateObservableId.ON_ERROR].Signal();
		}

		[Button]
		void CleanGenerator() {
			new ConfigCleaner().Clean(_config);
			new TilemapCleaner().Clean(_config);
			new ColliderGameObjectCleaner().Clean(gameObject);
			new MeshCleaner().Clean(gameObject);
			new GridCleaner().Clean(_config);
			new GraphCleaner().Clean();
			new EnsureMapFitsOnStack().Ensure(_config);
		}

		async UniTask HandleGeneratorDidNotRun() {
			// deserialize Astar data and try to parse serialized data to a pathfinding gridGraph
			await NodeSerializationSolver.FireTask(CancellationToken);
		}

		[Button]
		void InitializeGenerator() {
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
			GeneratedCollidersObj   = new EdgeColliderCreator().Create(this);
			MeshAndColliderSolver   = new SimpleMeshAndColliderSolver(_config, GeneratedCollidersObj, this, StopWatch);
			GridGraphBuilder        = new GridGraphBuilder(_config);
			NavGraphRulesSolver     = new NavGraphRulesSolver(_config);
			GraphScanner            = new GraphScanner(StopWatch);
			AstarSerializer         = new AstarSerializer(StopWatch);

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
	}
}