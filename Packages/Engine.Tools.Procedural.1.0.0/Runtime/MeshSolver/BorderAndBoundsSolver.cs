using CommunityToolkit.HighPerformance;

namespace Engine.Procedural.Runtime {
	public abstract class BorderAndBoundsSolver {
		public abstract int[,] DetermineBorderMap(Span2D<int> map);
	}
}