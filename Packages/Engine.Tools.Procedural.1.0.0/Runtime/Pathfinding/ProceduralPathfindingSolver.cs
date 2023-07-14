// using System;
// using System.Linq;
// using System.Threading;
// using BCL;
// using Cysharp.Threading.Tasks;
// using Engine.Procedural.Jobs;
// using Sirenix.OdinInspector;
// using Sirenix.Utilities;
// using Source;
// using Source.Events;
// using StateMachine;
// using UnityBCL;
// using UnityEngine;
//
// namespace Engine.Procedural {
// 	public interface IProceduralPathfindingSolver {
// 	}
//
// 	public class ProceduralPathfindingSolver : Singleton<ProceduralPathfindingSolver, IProceduralPathfindingSolver>,
// 	                                           IProgress, ICreation, IValidate,
// 	                                           IEngineEventListener<MeshGenerationData>,
// 	                                           IEngineEventListener<EventStateChange<CreationState>> {
// 		const    string           AIPathfindingName         = "AI Ground Level Grid";
// 		const    string           PathfindingMeshGameObject = "Pathfinding Mesh Game Object";
// 		const    string           ScanStart                 = "scanStart";
// 		const    string           ScanComplete              = "scanComplete";
// 		readonly ILoggingProvider _logging = new UnityLogging(new object()); 
// 		
// 		PathfindingData _data;
//
// 		DetermineGridGraphWalkable _rule;
//
// 		[SerializeField] [HideIf("@_useDefaultScanSettings")]
// 		GraphScanFinalizeSettings _scanSettings;
//
// 		[SerializeField] [Title("Graph Scan Settings")]
// 		bool _useDefaultScanSettings;
//
// 		public GraphScanFinalizeSettings ScanSettings
// 			=> _useDefaultScanSettings ? GraphScanFinalizeSettings.Default() : _scanSettings;
//
// 		// [field: SerializeField]
// 		// [field: HideLabel]
// 		// public ProceduralPathfindingSolverMonobehaviorModel MonobehaviorModel { get; private set; }
// 		//
// 		// public ProceduralPathfindingSolverModel Model { get; private set; }
//
// 		// public UniTask Init(CancellationToken token) {
// 		// 	// Model = new ProceduralPathfindingSolverModel(ScanStart, ScanComplete);
// 		// 	// AstarPath.FindAstarPath();
// 		// 	//GraphScanSolver.RemoveGraphs();
// 		// 	return new UniTask();
// 		// }
//
// 		// public UniTask Enable(CancellationToken token) {
// 		// 	MonobehaviorModel.NumberOfErodedNodesPerTile =
// 		// 		Mathf.RoundToInt(1 / Mathf.Pow(MonobehaviorModel.ErosionGridNodeSize, 2));
// 		//
// 		// 	return new UniTask();
// 		// }
//
// 		// public UniTask Begin(CancellationToken token) => new();
// 		//
// 		// public UniTask End(CancellationToken token) => new();
//
// 		// public UniTask Dispose(CancellationToken token) {
// 		// 	this.StopListeningToEvents<MeshGenerationData>();
// 		// 	this.StopListeningToEvents<EventStateChange<CreationState>>();
// 		// 	// GraphScanSolver.RemoveGraphs();
// 		// 	// var o = MonobehaviorModel.PathfindingGameObject;
// 		// 	// o.GetComponentsInChildren(typeof(Transform)).DeleteObj(o);
// 		// 	return new UniTask();
// 		// }
//
// 		// public async UniTask Progress_CalculatingPathfinding(PathfindingProgressData progressData,
// 		// 	CancellationToken token) {
// 		// 	//Model.Observables[ScanStart].Signal();
// 		//
// 		// 	// _data = new PathfindingData(MonobehaviorModel);
// 		// 	// var astarData = new GetActiveAstarData().Retrieve(); 
// 		// 	// 	GraphScanSolver.GetAstarActiveData();
// 		//
// 		// 	// var buildData = new GridGraphBuilder.Data(
// 		// 	// 	AIPathfindingName,
// 		// 	// 	progressData.MapDimensions,
// 		// 	// 	MonobehaviorModel.ErosionCollisionDiameter,
// 		// 	// 	MonobehaviorModel.ErosionObstacleLayerMasks,
// 		// 	// 	MonobehaviorModel.ErosionGridNodeSize,
// 		// 	// 	progressData.CellSize);
// 		//
// 		// 	//var gridGraph = GridGraphBuilder.BuildGridGraph(astarData, buildData);
// 		// 	
// 		// 	//new GridGraphRuleRemover().Remove(gridGraph);
// 		// 	// gridGraph.rules.AddRule(new WalkabilityRule(progressData.BoundaryTilemap, progressData.GroundTilemap,
// 		// 	// 	progressData.CellSize));
// 		//
// 		// 	// var erosionData = new SetErodedNodes.Data(
// 		// 	// 	MonobehaviorModel.ErodePathfindingGrid,
// 		// 	// 	MonobehaviorModel.NodesToErodeAtBoundaries,
// 		// 	// 	MonobehaviorModel.StartingNodeIndexToErode);
// 		//
// 		// 	//await SetErodedNodes.Erode(gridGraph, erosionData);
// 		// 	// await GraphScanner.ScanGraphAsync(gridGraph, token, progressData.ProceduralGenerator);
// 		// 	// Model.Observables[ScanComplete].Signal();
// 		//
// 		// 	//SerializeGraph(progressData.Seed, progressData.Iteration, progressData.NameOfMap);
// 		// 	//new GraphScanFinalize().Finalize(gridGraph, ScanSettings);
// 		// }
//
// 		// public UniTask Progress_PopulatingMap(CancellationToken token) => new();
// 		//
// 		// public UniTask Progress_SmoothingMap(CancellationToken token) => new();
// 		//
// 		// public UniTask Progress_CreatingBoundary(CancellationToken token) => new();
// 		//
// 		// public UniTask Progress_RemovingRegions(CancellationToken token) => new();
// 		//
// 		// public UniTask Progress_CompilingData(CancellationToken token) => new();
// 		//
// 		// public UniTask Progress_PreparingAndSettingTiles(CancellationToken token) => new();
// 		//
// 		// public UniTask Progress_GeneratingMesh(CancellationToken token) => new();
//
// 		// public void ValidateShouldQuit() {
// 		// 	var exitHandler = new ProceduralExitHandler();
// 		//
// 		// 	var statements = new Func<bool>[] {
// 		// 		() => MonobehaviorModel.PathfindingGameObject == null,
// 		// 		() => MonobehaviorModel.ErosionObstacleLayerMasks.IsNullOrEmpty()
// 		// 	};
// 		//
// 		// 	exitHandler.DetermineQuit(statements);
// 		// }
//
// 		/// <summary>
// 		///     Listens for an event fired by the procedural mesh generator. Once it hears this event, it caches the pathfinding
// 		///     mesh
// 		///     and procedurally calculates an A* algorithm for pathfinding.
// 		/// </summary>
// 		/// <param name="e"></param>
// // 		public void OnEventHeard(MeshGenerationData e) {
// // 			// TODO: what was the intent for caching the generated mesh? Is this code erroneous?
// // 			// _pathfindingMesh           = e.GeneratedMesh;
// // 			// _pathfindingMesh.triangles = _pathfindingMesh.triangles.Reverse().ToArray();
// //
// // 			if (Model.PathfindingMeshGameObject) {
// // #if UNITY_EDITOR
// // 				DestroyImmediate(Model.PathfindingMeshGameObject);
// // #else
// // 				Destroy(Model.PathfindingMeshGameObject);
// // #endif
// // 			}
// //
// // 			Model.PathfindingMeshGameObject = new GameObject {
// // 				name = PathfindingMeshGameObject, layer = LayerMask.NameToLayer("Obstacles")
// // 			};
// //
// // 			var pathfindingMeshFilter = Model.PathfindingMeshGameObject.AddComponent<MeshFilter>();
// // 			pathfindingMeshFilter.mesh = e.GeneratedMesh;
// //
// // 			Model.PathfindingMeshGameObject.transform.parent = MonobehaviorModel.PathfindingGameObject.transform;
// // 		}
// //
// // 		public void OnEventHeard(EventStateChange<CreationState> e) {
// // 		}
//
// 		// void SerializeGraph(string seed, int iteration, string nameOfMap) {
// 		// 	// if (!MonobehaviorModel.SerializerSetup) {
// 		// 	// 	_logging.Warning("Generated graph is not being serialized. Please add/assign a SerializerSetup component.");
// 		// 	// 	return;
// 		// 	// }
// 		// 	//
// 		// 	// if (string.IsNullOrWhiteSpace(nameOfMap))
// 		// 	// 	nameOfMap = "proceduralMap";
// 		//
// 		// 	// AstarSerializer.SerializeCurrentAstarGraph(MonobehaviorModel.SerializerSetup,
// 		// 	// 	MonobehaviorModel.SavePrefix + nameOfMap + "_" + seed + "_luid" + iteration);
// 		// }
//
// // 		void OnDrawGizmosSelected() {
// // #if UNITY_EDITOR
// // 			_data?.DrawGizmos();
// // #endif
// // 		}
// //
// // 		void OnEnable() {
// // 			this.StartListeningToEvents<MeshGenerationData>();
// // 			this.StartListeningToEvents<EventStateChange<CreationState>>();
// // 		}
// //
// // 		void OnDisable() {
// // 			this.StopListeningToEvents<MeshGenerationData>();
// // 			this.StopListeningToEvents<EventStateChange<CreationState>>();
// // 		}
//
// 		// [Button(ButtonSizes.Medium, Name = "Force Update Settings")]
// 		// void ForceUpdateScanSettings() {
// 		// 	if (AstarPath.active != null && AstarPath.active.data != null) {
// 		// 		var gridGraph = AstarPath.active.data.gridGraph;
// 		//
// 		// 		if (gridGraph == null)
// 		// 			return;
// 		//
// 		// 		new GraphScanFinalize().Finalize(gridGraph, ScanSettings);
// 		// 	}
// 		// }
//
// #if UNITY_EDITOR || UNITY_STANDALONE
// 		// bool ShouldFix {
// 		// 	get {
// 		// 		var monobehaviorList = MonobehaviorModel?.GetPropertyValues<AstarPath>();
// 		// 		if (monobehaviorList == null || monobehaviorList.Count < 1)
// 		// 			return false;
// 		// 		
// 		// 		return monobehaviorList.Any(x => x == null);
// 		// 	}
// 		// }
//
// 		//
// 		// [Button]
// 		// [ShowIf("@ShouldFix")]
// 		// void Fix() {
// 		// 	FixPathfinder();
// 		// }
// 		//
// 		// void FixPathfinder() {
// 		// 	if (MonobehaviorModel == null || MonobehaviorModel.PathfindingGameObject || Application.isPlaying)
// 		// 		return;
// 		//
// 		// 	if (!MonobehaviorModel.PathfindingGameObject) {
// 		// 		var sceneObj = GameObject.FindWithTag("Pathfinding");
// 		//
// 		// 		if (sceneObj) MonobehaviorModel.PathfindingGameObject = sceneObj.FixComponent<AstarPath>().gameObject;
// 		// 	}
// 		//
// 		// 	if (!MonobehaviorModel.PathfindingGameObject) {
// 		// 		var obj = FindObjectOfType<AstarPath>();
// 		//
// 		// 		if (!obj) {
// 		// 			var pObj = new GameObject("Pathfinder") {
// 		// 				transform = {
// 		// 					position = Vector3.zero
// 		// 				},
// 		// 				tag = "Pathfinding"
// 		// 			};
// 		//
// 		// 			pObj.AddComponent<AstarPath>();
// 		// 			MonobehaviorModel.PathfindingGameObject = pObj;
// 		// 		}
// 		// 	}
// 		// }
// #endif
// 	}
// }