using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural {
	public class MarchingSquaresMeshTriangulationSolver : MeshTriangulationSolver {
		HashSet<int>                    CheckedVertices { get; }
		SquareGrid                      SquareGrid      { get; }
		Dictionary<int, List<Triangle>> TriangleTracker { get; }
		MeshFilter                      MeshFilter      { get; }

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

			for (var x = 0; x < xLength; x++) {
				for (var y = 0; y < yLength; y++)
					_triangulationAlgorithm.TriangulateSquare(
						SquareGrid.Squares[x, y], CheckedVertices, TriangleTracker);
			}
		}

		void SolveMesh(int[,] mapBorder) {
			var mesh = new Mesh { name = Constants.MESH_LABEL };
			MeshFilter.mesh = mesh;

			var vertices = _triangulationAlgorithm.GetWalkableVertices;
			mesh.vertices  = vertices.ToArray();
			mesh.triangles = _triangulationAlgorithm.GetWalkableTriangles.ToArray();
			mesh.uv        = _uvSolver.CalculateUVs(mapBorder, vertices, Constants.CELL_SIZE);
			mesh.RecalculateNormals();

			SolvedMesh = mesh;
		}


		public MarchingSquaresMeshTriangulationSolver(ProceduralConfig config) {
			_uvSolver               = new ProceduralMeshUVSolver();
			_triangulationAlgorithm = new TriangulationAlgorithm();
			Outlines                = new List<List<int>>();
			SquareGrid              = new SquareGrid();
			CheckedVertices         = new HashSet<int>();
			TriangleTracker         = new Dictionary<int, List<Triangle>>();
			MeshFilter              = config.MeshFilter;
		}

		readonly TriangulationAlgorithm _triangulationAlgorithm;
		readonly ProceduralMeshUVSolver _uvSolver;
	}
}