// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using Cysharp.Threading.Tasks;
// using Engine.Procedural.ColliderSolver;
// using Sirenix.OdinInspector;
// using UnityBCL;
// using UnityEngine;
//
// namespace Engine.Procedural {
// 	[RequireComponent(typeof(MeshFilter))]
// 	public class ProceduralMeshSolver : Singleton<ProceduralMeshSolver, IProceduralMeshSolver>,
// 	                                    ICreation, IProgress, IValidate {
// 		[SerializeField] [HideInInspector] MeshGenerationData _cachedMeshGeneratedData;
//
// 		GameObject              _colliderGameObject;
// 		MeshTriangulationSolver _meshTriangulationSolver;
//
// 		public MeshGenerationData GetGeneratedData => _cachedMeshGeneratedData;
//
// 		bool ShouldFix => MonoModel != null && MonoModel.GetPropertyValues<object>().Any(x => x == null);
//
// 		[field: SerializeField]
// 		[field: HideLabel]
// 		public ProceduralMeshSolverMonobehaviorModel MonoModel { get; private set; }
//
// 		[field: SerializeField] public ProceduralMeshSolverModel Model { get; private set; }
//
// 		// public UniTask Init(CancellationToken token) {
// 		// 	Model = new ProceduralMeshSolverModel {
// 		// 		EdgeColliders = new EdgeCollider2D[50]
// 		// 	};
// 		// 	return new UniTask();
// 		// }
//
// 		//public UniTask Enable(CancellationToken token) => new();
//
// 		// public UniTask Begin(CancellationToken token) => new();
// 		//
// 		// public UniTask End(CancellationToken token) => new();
//
// 		// public UniTask Dispose(CancellationToken token) {
// 		// 	// gameObject.GetComponentsInChildren(typeof(EdgeCollider2D)).DeleteObj();
// 		// 	// gameObject.GetComponentsInChildren(typeof(BoxCollider)).DeleteObj();
// 		// 	// gameObject.GetComponentsInChildren(typeof(Transform)).DeleteObj(gameObject);
// 		// 	// MonoModel.MeshFilter.mesh = null;
// 		// 	// return new UniTask();
// 		// }
// 		//
// 		// public UniTask Progress_PopulatingMap(CancellationToken token)    => new();
// 		// public UniTask Progress_SmoothingMap(CancellationToken token)     => new();
// 		// public UniTask Progress_CreatingBoundary(CancellationToken token) => new();
// 		// public UniTask Progress_RemovingRegions(CancellationToken token)  => new();
// 		// public UniTask Progress_CompilingData(CancellationToken token)    => new();
//
// 		// public UniTask Progress_PreparingAndSettingTiles(CancellationToken token) {
// 		// 	var o = new GameObject("Procedural Edge Colliders") {
// 		// 		transform = {
// 		// 			parent = transform
// 		// 		},
// 		// 		layer = LayerMask.NameToLayer("Obstacles")
// 		// 	};
// 		//
// 		// 	o.transform.localPosition = o.transform.parent.position;
// 		// 	_colliderGameObject       = o;
// 		//
// 		// 	return new UniTask();
// 		// }
//
// 		// public async UniTask Progress_GeneratingMesh(CancellationToken token) {
// 		// 	var mapConfig          = MonoModel.ProceduralMapSolver.MonoModel.ProceduralConfig;
// 		// 	var triangulationModel = new MeshTriangulationSolverModel(mapConfig, MonoModel.MeshFilter);
// 		//
// 		// 	var borderMap = MonoModel.ProceduralMapSolver.Model.BorderMap;
// 		// 	_meshTriangulationSolver = new MarchingSquaresMeshTriangulationSolver(triangulationModel, borderMap);
// 		// 	var config = MonoModel.ProceduralMeshSolverConfiguration;
// 		// 	var (triangles, vertices) = await _meshTriangulationSolver.Triangulate(borderMap, token);
// 		//
// 		// 	await SolverColliderCreation(config, vertices, mapConfig.ObstacleLayerMask, mapConfig.BoundaryLayerMask);
// 		//
// 		// 	// var roomMeshModel = new RoomMeshSerializationSolverModel(_meshTriangulationSolver.Outlines, vertices);
// 		// 	// var col           = RoomMeshSerializationSolver.GenerateMeshes(roomMeshModel);
// 		// 	var roomMeshes = new RoomMeshDictionary();
// 		//
// 		// 	_cachedMeshGeneratedData = new GenerationData(
// 		// 		_meshTriangulationSolver.SolvedMesh, true, mapConfig.Seed, roomMeshes);
// 		//
// 		// 	var characteristics = new MapCharacteristics(_meshTriangulationSolver.Outlines, vertices);
// 		// 	ProceduralGenerationEvents.Global.Hook.MapCharacteristicsCalculated?.Invoke(characteristics);
// 		// 	var proxy = new InternalEventProxy();
// 		// 	proxy.TriggerEvent(_cachedMeshGeneratedData);
// 		// }
//
// 		// public UniTask Progress_CalculatingPathfinding(
// 		// 	PathfindingProgressData progressData, CancellationToken token) => new();
//
// 		// public void ValidateShouldQuit() {
// 		// 	var exitHandler = new ProceduralExitHandler();
// 		//
// 		// 	var statements = new Func<bool>[] {
// 		// 		() => MonoModel.ProceduralMapSolver               == null,
// 		// 		() => MonoModel.ProceduralMeshSolverConfiguration == null
// 		// 	};
// 		//
// 		// 	exitHandler.DetermineQuit(statements);
// 		// }
//
// 		// public UniTask Progress_CalculatingPathfinding(CancellationToken token) => new();
// 		//
// 		// async UniTask SolverColliderCreation(ProceduralMeshSolverConfiguration config, List<Vector3> vertices,
// 		// 	LayerMask obstacleLayer, LayerMask boundary) {
// 		// 	ColliderSolver.ColliderSolver solver;
// 		// 	var solverModel = new ColliderSolverModel(_meshTriangulationSolver.Outlines, _colliderGameObject,
// 		// 		vertices, obstacleLayer, boundary);
// 		//
// 		// 	if (config.CollisionSolverType == ColliderSolverType.Box)
// 		// 		solver = new BoxColliderSolver(config.BoxColliderModel, gameObject);
// 		//
// 		// 	else if (config.CollisionSolverType == ColliderSolverType.Edge)
// 		// 		solver = new EdgeColliderSolver(config.EdgeColliderModel, Model.EdgeColliders);
// 		//
// 		// 	else
// 		// 		solver = new PrimitiveColliderSolver(MonoModel.ProceduralMeshSolverConfiguration
// 		// 		                                              .PrimitiveColliderModel);
// 		//
// 		// 	await solver.CreateCollider(solverModel);
// 		// }
//
// 		// [Button]
// 		// [ShowIf("@ShouldFix")]
// 		// void Fix() {
// 		// 	MonoModel.MeshFilter          = gameObject.FixComponent<MeshFilter>();
// 		// 	MonoModel.ProceduralMapSolver = gameObject.FixComponent<ProceduralMapSolver>();
// 		// }
// 	}
// }