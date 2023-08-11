// Engine.Procedural

using System;

namespace ProceduralGeneration {
	public class MarchingSquaresMeshSolver : MeshSolver {
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

		public MarchingSquaresMeshSolver(ISeed info) {
			_meshTriangulationSolver = new MarchingSquaresMeshTriangulationSolver(info);
		}

		readonly MeshTriangulationSolver _meshTriangulationSolver;
	}
}