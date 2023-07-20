// Engine.Procedural

using System;
using System.Collections.Generic;
using BCL;
using UnityEngine;

namespace Engine.Procedural {
	public class MarchingSquaresMeshSolver : MeshSolver {
		ProceduralConfig   Config             { get; }
		StopWatchWrapper   StopWatch          { get; }
		GameObject         RootGameObject     { get; }
		GameObject         ColliderGameObject { get; }
		MeshFilter         MeshFilter         { get; }
		ISeedInfo          ProcGenInfo        { get; }
		ColliderSolverType CollisionType      { get; }
		LayerMask          BoundaryMask       { get; }
		LayerMask          ObstaclesMask      { get; }


		public override MeshSolverData SolveAndCreate(int[,] mapBorder) {
			var (triangles, vertices) = _meshTriangulationSolver.Triangulate(mapBorder);

			var roomMeshes = new RoomMeshDictionary();

			return new MeshSolverData(
				_meshTriangulationSolver.SolvedMesh,
				vertices,
				triangles,
				_meshTriangulationSolver.Outlines,
				roomMeshes
			);
		}

		public MarchingSquaresMeshSolver(
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