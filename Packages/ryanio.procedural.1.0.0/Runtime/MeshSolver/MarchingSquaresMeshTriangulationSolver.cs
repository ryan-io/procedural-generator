using System;
using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralGeneration {
	internal class MarchingSquaresMeshTriangulationSolver : MeshTriangulationSolver {
		HashSet<int>                    CheckedVertices  { get; }
		SquareGrid                      SquareGrid       { get; }
		Dictionary<int, List<Triangle>> TriangleTracker  { get; }
		string                          SerializableName { get; }

		internal override Tuple<List<int>, List<Vector3>> Triangulate(ref int[,] map) {
			SquareGrid.SetSquares(ref map);
			SetTriangles();
			SolveMesh(ref map);

			OutLineConnectionSolver.Solve(
				_triangulationAlgorithm.GetWalkableVertices, CheckedVertices, Outlines, TriangleTracker);

			var output = Tuple.Create(
				_triangulationAlgorithm.GetWalkableTriangles,
				_triangulationAlgorithm.GetWalkableVertices);

			return output;
		}

		void SetTriangles() {
			var xLength = SquareGrid.Squares.GetLength(0);
			var yLength = SquareGrid.Squares.GetLength(1);

			for (var x = 0; x < xLength; x++) {
				for (var y = 0; y < yLength; y++) {
					_triangulationAlgorithm.TriangulateSquare(
						SquareGrid.Squares[x, y], CheckedVertices, TriangleTracker);
				}
			}
		}

		void SolveMesh(ref int[,] map) {
			var mesh     = new Mesh { name = Constants.SAVE_MESH_PREFIX + SerializableName };
			var vertices = _triangulationAlgorithm.GetWalkableVertices;

			mesh.indexFormat = vertices.Count >= sizeof(UInt16) ? IndexFormat.UInt32 : IndexFormat.UInt16;
			mesh.vertices    = vertices.ToArray();
			mesh.triangles   = _triangulationAlgorithm.GetWalkableTriangles.ToArray();
			mesh.uv          = ProceduralMeshUVSolver.CalculateUVs(ref map, vertices, Constants.Instance.CellSize);
			mesh.RecalculateNormals();

			SolvedMesh = mesh;
		}


		internal MarchingSquaresMeshTriangulationSolver(MeshSolverCtx ctx) {
			SerializableName        = ctx.SerializableName;
			_uvSolver               = new ProceduralMeshUVSolver();
			_triangulationAlgorithm = new TriangulationAlgorithm();
			Outlines                = new List<List<int>>();
			SquareGrid              = new SquareGrid();
			CheckedVertices         = new HashSet<int>();
			TriangleTracker         = new Dictionary<int, List<Triangle>>();
		}

		readonly TriangulationAlgorithm _triangulationAlgorithm;
		readonly ProceduralMeshUVSolver _uvSolver;
	}
}