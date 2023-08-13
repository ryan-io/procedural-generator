using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal class MarchingSquaresSmoothMapSolver : SmoothMapSolver {
		SmoothMapSolverCtx Ctx     { get; set; }

		/// <summary>
		/// Will take the primary map span, make a copy, performed calculations over the entire mapSpanCopy
		/// The number of times to perform all calculations = SmoothingIterations
		/// Once each iteration is complete, it will copy all items in mapSpanCopy to mapSpan and repeat
		/// </summary>
		/// <param name="map">The primary map span</param>
		internal override void Smooth(Span2D<int> map) {
			var copy       = new Span2D<int>(map.ToArray());

			// SmoothingIterations is relatively small in all scenarios
			for (var i = 0; i < Ctx.SmoothingIterations; i++) {
				map = GetSmoothedMap(map, copy);
			}
		}

		Span2D<int> GetSmoothedMap(Span2D<int> original, Span2D<int> copy) {
			for (var i = 0; i < Ctx.Dimensions.Rows * Ctx.Dimensions.Columns; i++) {
				var row   = i / Ctx.Dimensions.Columns;
				var colum = i % Ctx.Dimensions.Columns;

				copy = DetermineNeighborLimits(row, colum, copy, original);
			}

			return copy;
		}

		Span2D<int> DetermineNeighborLimits(int x, int y, Span2D<int> original, Span2D<int> copy) {
			var surroundingWalls = GetAdjacentWallsCount(x, y, original);

			if (surroundingWalls > Ctx.UpperNeighborLimit)
				copy[x, y] = 1;

			else if (surroundingWalls < Ctx.LowerNeighborLimit)
				copy[x, y] = 0;

			return copy;
		}

		int GetAdjacentWallsCount(int x, int y, Span2D<int> original) {
			var count = 0;

			for (var neighborX = x - 1; neighborX <= x + 1; neighborX++) {
				for (var neighborY = y - 1; neighborY <= y + 1; neighborY++)
					count = DetermineCount(x, y, neighborX, neighborY, count, original);
			}

			return count;
		}

		int DetermineCount(int gridX, int gridY, int neighborX, int neighborY, int count, Span2D<int> original) {
			if (IsWithinBoundary(neighborX, neighborY)) {
				if (neighborX != gridX || neighborY != gridY)
					count += original[neighborX, neighborY];
			}

			else {
				count++;
			}

			return count;
		}

		bool IsWithinBoundary(int neighborX, int neighborY) {
			return neighborX >= 0 && neighborX < Ctx.Dimensions.Rows && neighborY >= 0 &&
			       neighborY < Ctx.Dimensions.Columns;
		}

		internal MarchingSquaresSmoothMapSolver(SmoothMapSolverCtx ctx) {
			Ctx = ctx;
		}
	}
}