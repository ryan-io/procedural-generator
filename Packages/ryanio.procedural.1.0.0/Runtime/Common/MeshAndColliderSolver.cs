// Engine.Procedural

using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal abstract class MeshSolver {
		internal abstract MeshData Create(Span2D<int> map);
	}
}