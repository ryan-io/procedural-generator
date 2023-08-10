using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	public abstract class TileTypeSolver {
		public abstract void        SetTiles(Span2D<int> span);
	}
}