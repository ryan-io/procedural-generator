// Engine.Procedural

using System;
using CommunityToolkit.HighPerformance;
using UnityEngine;

namespace ProceduralGeneration {
	internal class MarchingSquaresMeshSolver : MeshSolver {
		GameObject Owner            { get; }
		string     SerializableName { get; }
		
		internal override MeshSolverData Create(Span2D<int> map) {
			var (triangles, vertices) = _meshTriangulationSolver.Triangulate(map.ToArray());
			var roomMeshes = new RoomMeshDictionary();

			var data = new MeshSolverData(
				_meshTriangulationSolver.SolvedMesh,
				vertices,
				triangles,
				_meshTriangulationSolver.Outlines,
				roomMeshes
			);

			var meshRenderer = new MeshRendering(Owner, default);	// TODO: add material
			meshRenderer.Render(data, Constants.SAVE_MESH_PREFIX + SerializableName);

			return data;
		}

		internal MarchingSquaresMeshSolver(MeshSolverCtx ctx) {
			Owner                    = ctx.Owner;
			SerializableName         = ctx.SerializableName;
			_meshTriangulationSolver = new MarchingSquaresMeshTriangulationSolver(ctx);
		}

		readonly MeshTriangulationSolver _meshTriangulationSolver;
	}
}