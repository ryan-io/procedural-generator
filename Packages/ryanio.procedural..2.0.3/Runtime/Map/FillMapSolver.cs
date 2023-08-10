using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	public abstract class FillMapSolver {
		public abstract void Fill(Span2D<int> primaryMap);
	}
}