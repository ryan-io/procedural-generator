// Engine.Procedural

using System;
using System.Collections.Generic;
using BCL;
using UnityEngine;

namespace Engine.Procedural {
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
			CollisionType            = config.GeneratedColliderType;
			ColliderGameObject       = colliderObj;
			RootGameObject           = procGen.gameObject;
			ProcGenInfo              = procGen;
			StopWatch                = stopWatch;
		}

		readonly MeshTriangulationSolver _meshTriangulationSolver;
	}
}