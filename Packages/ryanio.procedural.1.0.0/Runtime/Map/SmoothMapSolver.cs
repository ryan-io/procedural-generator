using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal abstract class SmoothMapSolver {
		internal abstract void Smooth(Span2D<int> map);
	}
}