using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	public class MarchingSquaresMeshTriangulationSolver : MeshTriangulationSolver {
		HashSet<int>                    CheckedVertices { get; }
		SquareGrid                      SquareGrid      { get; }
		Dictionary<int, List<Triangle>> TriangleTracker { get; }
		ISeedInfo                       Info            { get; }

		public override Tuple<List<int>, List<Vector3>> Triangulate(int[,] mapBorder) {
			SquareGrid.SetSquares(mapBorder);
			SetTriangles();
			SolveMesh(mapBorder);
			OutLineConnectionSolver.Solve(
				_triangulationAlgorithm.GetWalkableVertices, CheckedVertices, Outlines, TriangleTracker);

			var output = Tuple.Create(
				_triangulationAlgorithm.GetWalkableTriangles,
				_triangulationAlgorithm.GetWalkableVertices);

			return output;
		}

		void ClearLists() {
			Outlines.Clear();
			CheckedVertices.Clear();
			TriangleTracker.Clear();
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

			
			// for (var x = 0; x < xLength; x++) {
			// 	for (var y = 0; y < yLength; y++)
			// 		_triangulationAlgorithm.TriangulateSquare(
			// 			SquareGrid.Squares[x, y], CheckedVertices, TriangleTracker);
			// }
		}

		void SolveMesh(int[,] mapBorder) {
			var mesh     = new Mesh { name = Constants.SAVE_MESH_PREFIX + Info.CurrentSerializableName };
			var vertices = _triangulationAlgorithm.GetWalkableVertices;
			
			mesh.vertices  = vertices.ToArray();
			mesh.triangles = _triangulationAlgorithm.GetWalkableTriangles.ToArray();
			mesh.uv        = _uvSolver.CalculateUVs(mapBorder, vertices, Constants.CELL_SIZE);
			mesh.RecalculateNormals();

			SolvedMesh = mesh;
		}


		public MarchingSquaresMeshTriangulationSolver(ISeedInfo info) {
			Info                    = info;
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