// Engine.Procedural

using System;

namespace Engine.Procedural.Runtime {
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

		public MarchingSquaresMeshSolver(ProceduralConfig config) {
			_meshTriangulationSolver = new MarchingSquaresMeshTriangulationSolver(config);
		}

		readonly MeshTriangulationSolver _meshTriangulationSolver;
	}
}