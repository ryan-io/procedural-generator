using CommunityToolkit.HighPerformance;

namespace Engine.Procedural {
	public abstract class RegionRemovalSolver {
		public abstract void Remove(Span2D<int> primarySpan);
	}
}