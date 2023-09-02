// Engine.Procedural

using System;
using CommunityToolkit.HighPerformance;
using JetBrains.Annotations;
using UnityEngine;

namespace ProceduralGeneration {
	internal class MarchingSquaresMeshSolver : MeshSolver {
		GameObject           Owner            { get; }
		[CanBeNull] Material MeshMaterial     { get; }
		string               SerializableName { get; }
		
		internal override MeshData Create(Span2D<int> map) {
			var (triangles, vertices) = _meshTriangulationSolver.Triangulate(map.ToArray());
			var roomMeshes = new RoomMeshDictionary();

			var data = new MeshData(
				_meshTriangulationSolver.SolvedMesh,
				vertices,
				triangles,
				_meshTriangulationSolver.Outlines,
				roomMeshes
			);

			var meshRenderer = new MeshRendering(Owner, MeshMaterial); 
			meshRenderer.Render(data, Constants.SAVE_MESH_PREFIX + SerializableName);

			return data;
		}

		internal MarchingSquaresMeshSolver(MeshSolverCtx ctx) {
			Owner                    = ctx.Owner;
			SerializableName         = ctx.SerializableName;
			MeshMaterial             = ctx.MeshMaterial;
			_meshTriangulationSolver = new MarchingSquaresMeshTriangulationSolver(ctx);
		}

		readonly MeshTriangulationSolver _meshTriangulationSolver;
	}
}