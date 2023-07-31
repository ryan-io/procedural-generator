using CommunityToolkit.HighPerformance;

namespace Engine.Procedural.Runtime {
	public abstract class TileTypeSolver {
		public abstract void        SetTiles(Span2D<int> span);
	}
}