using UnityEngine;

namespace Engine.Procedural {
	public readonly struct MeshTriangulationSolverModel {
		public MeshFilter MeshFilter { get; }
		public float      SquareSize { get; }

		public MeshTriangulationSolverModel(
			ProceduralConfig proceduralConfig, MeshFilter meshFilter) {
			SquareSize = ProceduralConfig.CELL_SIZE;
			MeshFilter = meshFilter;
		}
	}
}