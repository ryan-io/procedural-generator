using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal class MarchingSquaresMeshTriangulationSolver : MeshTriangulationSolver {
		HashSet<int>                    CheckedVertices  { get; }
		SquareGrid                      SquareGrid       { get; }
		Dictionary<int, List<Triangle>> TriangleTracker  { get; }
		string                          SerializableName { get; }

		internal override Tuple<List<int>, List<Vector3>> Triangulate(int[,] map) {
			SquareGrid.SetSquares(map);
			SetTriangles();
			SolveMesh(map);
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


			for (var i = 0; i < xLength * yLength; i++) {
				var row    = i / yLength;
				var column = i % yLength;

				_triangulationAlgorithm.TriangulateSquare(
					SquareGrid.Squares[row, column], CheckedVertices, TriangleTracker);
			}
		}

		void SolveMesh(int[,] map) {
			var mesh     = new Mesh { name = Constants.SAVE_MESH_PREFIX + SerializableName };
			var vertices = _triangulationAlgorithm.GetWalkableVertices;

			mesh.vertices  = vertices.ToArray();
			mesh.triangles = _triangulationAlgorithm.GetWalkableTriangles.ToArray();
			mesh.uv        = ProceduralMeshUVSolver.CalculateUVs(map, vertices, Constants.CELL_SIZE);
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