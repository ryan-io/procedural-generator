using CommunityToolkit.HighPerformance;

namespace Engine.Procedural {
	public abstract class BorderAndBoundsSolver {
		public abstract int[,] DetermineBorderMap(Span2D<int> map);
	}
}