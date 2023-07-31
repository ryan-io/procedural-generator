using CommunityToolkit.HighPerformance;

namespace Engine.Procedural.Runtime {
	public abstract class FillMapSolver {
		public abstract void Fill(Span2D<int> primaryMap);
	}
}