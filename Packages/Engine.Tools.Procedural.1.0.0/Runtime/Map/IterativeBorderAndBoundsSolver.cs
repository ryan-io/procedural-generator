using BCL;
using CommunityToolkit.HighPerformance;

namespace Engine.Procedural {
	public class IterativeBorderAndBoundsSolver : BorderAndBoundsSolver {
		StopWatchWrapper StopWatch       { get; }
		int              NumberOfRows    { get; set; }
		int              NumberOfColumns { get; set; }
		int              MapBorderSize   { get; }
		int              BorderSize      { get; }

		public override unsafe int[,] DetermineBorderMap(Span2D<int> mapSpan) {
			NumberOfRows    = mapSpan.Height;
			NumberOfColumns = mapSpan.Width;
			
			var  borderWidth   = BorderSize * 2;
			int* borderPointer = stackalloc int[(NumberOfRows + borderWidth) * (NumberOfColumns + borderWidth)];
			
			var  borderSpan    = new Span2D<int>(
				borderPointer, 
				NumberOfRows + borderWidth, 
				NumberOfColumns + borderWidth, 
				0);
			
			for (var x = 0; x < NumberOfRows; x++) {
				for (var y = 0; y < NumberOfColumns; y++)
					borderSpan[x, y] = DetermineIfTileIsBorder(borderSpan, mapSpan, x, y);
			}

			return borderSpan.ToArray();
		}

		int DetermineIfTileIsBorder(Span2D<int> borderMapSpan, Span2D<int> mapSpan, int x, int y) {
			if (IsBorder(x, y))
				return mapSpan[x - MapBorderSize, y - MapBorderSize];

			return borderMapSpan[x, y] = 1;
		}

		bool IsBorder(int x, int y) => x >= MapBorderSize && x < NumberOfRows    + MapBorderSize &&
		                               y >= MapBorderSize && y < NumberOfColumns + MapBorderSize;

		public IterativeBorderAndBoundsSolver(ProceduralConfig config, StopWatchWrapper stopWatch) {
			MapBorderSize = config.BorderSize;
			BorderSize    = config.BorderSize;
			StopWatch     = stopWatch;
		}
	}
}