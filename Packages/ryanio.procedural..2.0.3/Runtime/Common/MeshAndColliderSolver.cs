// Engine.Procedural

using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal abstract class MeshSolver {
		internal abstract MeshSolverData Create(Span2D<int> map);
	}
}