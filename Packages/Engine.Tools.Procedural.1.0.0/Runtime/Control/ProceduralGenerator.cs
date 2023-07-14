using System;
using System.Collections.Generic;
using System.Threading;
using BCL;
using CommunityToolkit.HighPerformance;
using Cysharp.Threading.Tasks;
using Engine.Procedural.ColliderSolver;
using Engine.Tools.Serializer;
using Pathfinding;
using Sirenix.OdinInspector;
using StateMachine;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Engine.Procedural {
	public class NavGraphRulesSolver {
		Tilemap GroundTilemap   { get; }
		Tilemap BoundaryTilemap { get; }

		public void ResetGridGraphRules(GridGraph graph) {
			new GridGraphRuleRemover().Remove(graph);
		}

		public void SetGridGraphRules(GridGraph graph) {
			var walkabilityRule = new WalkabilityRule(BoundaryTilemap, GroundTilemap, Constants.CELL_SIZE);
			graph.rules.AddRule(walkabilityRule);
		}

		public NavGraphRulesSolver(ProceduralConfig config) {
			GroundTilemap   = config.TileMapDictionary[TileMapType.Ground];
			BoundaryTilemap = config.TileMapDictionary[TileMapType.Boundary];
		}
	}

	public abstract class NavGraphBuilder<T> where T : NavGraph {
		public abstract T Build();
	}

	public abstract class MeshAndColliderSolver {
		public abstract MeshGenerationData SolveAndCreate(int[,] mapBorder);
	}

	public class SimpleMeshAndColliderSolver : MeshAndColliderSolver {
		ProceduralConfig   Config             { get; }
		StopWatchWrapper   StopWatch          { get; }
		GameObject         RootGameObject     { get; }
		GameObject         ColliderGameObject { get; }
		MeshFilter         MeshFilter         { get; }
		ISeedInfo          ProcGenInfo        { get; }
		ColliderSolverType CollisionType      { get; }
		LayerMask          BoundaryMask       { get; }
		LayerMask          ObstaclesMask      { get; }


		public override MeshGenerationData SolveAndCreate(int[,] mapBorder) {
			var (triangles, vertices) = _meshTriangulationSolver.Triangulate(mapBorder);

			CreateColliders(vertices, ObstaclesMask, BoundaryMask);
			var roomMeshes = new RoomMeshDictionary();

			//TODO: any use for this?
			//var characteristics = new MapCharacteristics(_meshTriangulationSolver.Outlines, vertices);

			return new MeshGenerationData(_meshTriangulationSolver.SolvedMesh, roomMeshes, vertices, triangles);
		}

		void CreateColliders(List<Vector3> vertices, LayerMask obstacleLayer, LayerMask boundary) {
			CollisionSolver solver;

			var dto = new CollisionSolverDto(
				_meshTriangulationSolver.Outlines,
				ColliderGameObject,
				vertices,
				obstacleLayer,
				boundary);

			if (CollisionType == ColliderSolverType.Box)
				solver = new BoxCollisionSolver(Config, RootGameObject);

			else if (CollisionType == ColliderSolverType.Edge)
				solver = new EdgeCollisionSolver(Config, StopWatch);

			else
				solver = new PrimitiveCollisionSolver(Config);

			solver.CreateCollider(dto);
		}

		public SimpleMeshAndColliderSolver(
			ProceduralConfig config, GameObject colliderObj, ProceduralGenerator procGen, StopWatchWrapper stopWatch) {
			_meshTriangulationSolver = new MarchingSquaresMeshTriangulationSolver(config);
			Config                   = config;
			MeshFilter               = config.MeshFilter;
			ObstaclesMask            = config.ObstacleLayerMask;
			BoundaryMask             = config.BoundaryLayerMask;
			CollisionType            = config.NavGraphCollisionType;
			ColliderGameObject       = colliderObj;
			RootGameObject           = procGen.gameObject;
			ProcGenInfo              = procGen;
			StopWatch                = stopWatch;
		}

		readonly MeshTriangulationSolver _meshTriangulationSolver;
	}

	/// <summary>
	///     Verifies the scene contains the required components in order to run procedural generation logic
	///     This class will also kickoff the generation process
	///     *** The Procedural Generator requires unsafe context ***
	/// </summary>
	[RequireComponent(typeof(ProceduralSceneBounds))]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(ProceduralMeshSolver))]
	public class ProceduralGenerator : Singleton<ProceduralGenerator, ProceduralGenerator>,
	                                   ISeedInfo {
		[field: SerializeField] [field: Required]
		ProceduralConfig _config = null!;

		[field: SerializeField] [field: Required]
		SerializerSetup _generatorSerializer = null!;

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
				       _config.Name + 
				       Constants.UNDERSCORE + 
				       seedInfo.CurrentSeed +
				       Constants.UID +
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
		public unsafe void StartGeneration(bool alsoInitialize = true) {
			try {
#region CLEAN

				StateMachine.ChangeState(ProcessStep.Cleaning);
				Observables[StateObservableId.ON_CLEAN].Signal();
				CleanGenerator();

#endregion

#region INITIALIZE

				StateMachine.ChangeState(ProcessStep.Initializing);
				Observables[StateObservableId.ON_INIT].Signal();

				if (_config.RuntimeState == RuntimeState.DoNotGenerate)
					// do not proceed any further; redirect control elsewhere
					return;

				if (alsoInitialize)
					InitializeGenerator();

				var primaryPointer = stackalloc int[_config.Width * _config.Height];
				var primarySpan    = new Span2D<int>(primaryPointer, _config.Height, _config.Width, 0);

#endregion

#region RUN

				StateMachine.ChangeState(ProcessStep.Running);
				Observables[StateObservableId.ON_RUN].Signal();

				FillMapSolver.Fill(primarySpan);
				SmoothMapSolver.Smooth(primarySpan);
				RegionRemoverSolver.Remove(primarySpan);
				var mapBorder = BorderBoundsSolver.DetermineBorderMap(primarySpan);
				TileTypeSolver.SetTiles(primarySpan);
				new TileMapCompressor(_config.TilemapContainer).Compress();

				Tools.SetGridOrigin();
				Tools.SetGridScale(Constants.CELL_SIZE);

				//TODO: The reset of the  generation will utilize the previous array code instead of span
				var meshData = MeshAndColliderSolver.SolveAndCreate(mapBorder);
				//var meshData = MeshTriangulationSolver.Triangulate(mapBorder);
				new PrepPathfindingMesh().Prep(PathfindingMeshObj, meshData);
				var gridGraph = GridGraphBuilder.Build();
				NavGraphRulesSolver.ResetGridGraphRules(gridGraph);
				NavGraphRulesSolver.SetGridGraphRules(gridGraph);
				ErosionSolver.Erode(gridGraph);

				var scannerArgs = new GraphScanner.Args(
					gridGraph,
					() => {
						AstarSerializer.SerializeCurrentAstarGraph(_config.PathfindingSerializer, GetAstarSerializationName);
						
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
				var map = primarySpan.ToArray();
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
				GenLogging.LogWithTimeStamp(
					LogLevel.Error,
					StopWatch.TimeElapsed,
					e.Message,
					Message.CTX_ERROR);

				HandleErrorState();
			}

#region COMPLETE

			finally {
				StopWatch.Stop();
				StateMachine.ChangeState(ProcessStep.Completing);
				Observables[StateObservableId.ON_COMPLETE].Signal();

#endregion

#region DISPOSE

				// cleanup

				StateMachine.ChangeState(ProcessStep.Disposing);
				Observables[StateObservableId.ON_DISPOSE].Signal();

#endregion
			}
		}

		void HandleErrorState() {
			StateMachine.ChangeState(ProcessStep.Error);
			Observables[StateObservableId.ON_ERROR].Signal();
		}

		void CleanGenerator() {
			new ConfigCleaner().Clean(_config);
			new TilemapCleaner().Clean(_config);
			new ColliderGameObjectCleaner().Clean(gameObject);
			new MeshCleaner().Clean(gameObject);
			new GridCleaner().Clean(_config);
			new GraphCleaner().Clean();
		}

		async UniTask HandleGeneratorDidNotRun() {
			// deserialize Astar data and try to parse serialized data to a pathfinding gridGraph
			await NodeSerializationSolver.FireTask(CancellationToken);
		}

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
			NodeSerializationSolver = new NodeSerializationSolver(_config, _generatorSerializer, this, StopWatch);
			RegionRemoverSolver     = new FloodRegionRemovalSolver(_config);
			ErosionSolver           = new ErosionSolver(_config);
			GeneratedCollidersObj   = new EdgeColliderCreator().Create(this);
			MeshAndColliderSolver   = new SimpleMeshAndColliderSolver(_config, GeneratedCollidersObj, this, StopWatch);
			GridGraphBuilder        = new GridGraphBuilder(_config);
			NavGraphRulesSolver     = new NavGraphRulesSolver(_config);
			GraphScanner            = new GraphScanner(StopWatch);
			AstarSerializer         = new AstarSerializer();

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