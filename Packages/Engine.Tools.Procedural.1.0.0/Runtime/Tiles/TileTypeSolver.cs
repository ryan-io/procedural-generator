using CommunityToolkit.HighPerformance;

namespace Engine.Procedural {
	public abstract class TileTypeSolver {
		public abstract void        SetTiles(Span2D<int> span);
	}
}