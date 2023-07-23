using CommunityToolkit.HighPerformance;

namespace Engine.Procedural.Runtime {
	public abstract class SmoothMapSolver {
		public abstract void Smooth(Span2D<int> primaryMap);
	}
}