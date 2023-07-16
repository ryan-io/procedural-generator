using CommunityToolkit.HighPerformance;

namespace Engine.Procedural {
	public abstract class SmoothMapSolver {
		public abstract void Smooth(Span2D<int> mapSpan);
	}
}