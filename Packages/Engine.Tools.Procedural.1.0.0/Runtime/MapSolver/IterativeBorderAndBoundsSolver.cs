using BCL;
using CommunityToolkit.HighPerformance;

namespace Engine.Procedural {
	public class IterativeBorderAndBoundsSolver : BorderAndBoundsSolver {
		StopWatchWrapper StopWatch     { get; }
		int              MapWidth      { get; }
		int              MapHeight     { get; }
		int              MapBorderSize { get; }
		int              BorderSize    { get; }

		public override unsafe int[,] DetermineBorderMap(Span2D<int> mapSpan) {
			var borderWidth = BorderSize * 2;

			int* borderPointer = stackalloc int[(MapWidth + borderWidth) * (MapHeight + borderWidth)];
			var borderSpan = new Span2D<int>(
				borderPointer, MapHeight + borderWidth, MapWidth + borderWidth, 0);
			var lengthX = borderSpan.Width;
			var lengthY = borderSpan.Height;

			for (var x = 0; x < lengthX; x++) {
				for (var y = 0; y < lengthY; y++)
					borderSpan[x, y] = DetermineIfTileIsBorder(borderSpan, mapSpan, x, y);
			}

			return borderSpan.ToArray();
		}

		int DetermineIfTileIsBorder(Span2D<int> borderMapSpan, Span2D<int> mapSpan, int x, int y) {
			if (IsBorder(x, y))
				return mapSpan[x - MapBorderSize, y - MapBorderSize];

			return borderMapSpan[x, y] = 1;
		}

		bool IsBorder(int x, int y) => x >= MapBorderSize && x < MapWidth  + MapBorderSize &&
		                               y >= MapBorderSize && y < MapHeight + MapBorderSize;

		public IterativeBorderAndBoundsSolver(ProceduralConfig config, StopWatchWrapper stopWatch) {
			MapWidth      = config.Width;
			MapHeight     = config.Height;
			MapBorderSize = config.BorderSize;
			BorderSize    = config.BorderSize;
			StopWatch     = stopWatch;
		}
	}
}