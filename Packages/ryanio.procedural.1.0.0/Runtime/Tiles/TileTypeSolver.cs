using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal abstract class TileTypeSolver {
		internal abstract void Set(Span2D<int> span);
	}
}