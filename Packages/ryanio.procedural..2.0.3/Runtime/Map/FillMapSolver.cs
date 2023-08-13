using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal abstract class FillMapSolver {
		internal abstract void Fill(Span2D<int> primaryMap);
	}
}