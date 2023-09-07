using CommunityToolkit.HighPerformance;
using Unity.Profiling;

namespace ProceduralGeneration {
	internal class StandardSmoothMapSolver : SmoothMapSolver {
		SmoothMapSolverCtx Ctx { get; set; }

		/// <summary>
		/// Will take the primary map span, make a copy, performed calculations over the entire mapSpanCopy
		/// The number of times to perform all calculations = SmoothingIterations
		/// Once each iteration is complete, it will copy all items in mapSpanCopy to mapSpan and repeat
		/// </summary>
		/// <param name="map">The primary map span</param>
		internal override void Smooth(Span2D<int> map) {
			var copy = new Span2D<int>(map.ToArray());

			for (var i = 0; i < Ctx.SmoothingIterations; i++) {
				GetSmoothedMap(map, copy).CopyTo(map);
			}
		}

		static readonly ProfilerMarker m_GetSmoothedMap = new(nameof(GetSmoothedMap));

		Span2D<int> GetSmoothedMap(Span2D<int> original, Span2D<int> copy) {
			using (m_GetSmoothedMap.Auto()) {
				var rows    = original.Height;
				var columns = original.Width;
				
				// for (int i = 0; i < rows * columns; i++) {
				// 	int x = i % columns, y = i / rows;
				// 	copy = DetermineNeighborLimits( x,  y, copy, original);
				// }

				for (var x = 0; x < rows; x++) {
					for (var y = 0; y < columns; y++) {
						DetermineNeighborLimits(x, y, original, copy);
					}
				}

				return copy;
			}
		}

		const int COMPARISON_LIMIT = 4;

		static readonly ProfilerMarker m_DetermineNeighborLimits = new(nameof(DetermineNeighborLimits));

		void DetermineNeighborLimits(int x, int y, Span2D<int> original, Span2D<int> copy) {
			using (m_DetermineNeighborLimits.Auto()) {
				var surroundingWalls = GetAdjacentWallsCount(x, y, original);

				if (surroundingWalls > COMPARISON_LIMIT)
					copy[x, y] = 1;

				else if (surroundingWalls < COMPARISON_LIMIT)
					copy[x, y] = 0;

				//return copy;
			}
		}

		static readonly ProfilerMarker m_GetAdjacentWallsCount = new(nameof(GetAdjacentWallsCount));

		int GetAdjacentWallsCount(int x, int y, Span2D<int> original) {
			using (m_GetAdjacentWallsCount.Auto()) {
				var count = 0;

				for (var neighborX = x - 1; neighborX <= x + 1; neighborX++) {
					for (var neighborY = y - 1; neighborY <= y + 1; neighborY++)
						count = DetermineCount(x, y, neighborX, neighborY, count, original);
				}

				return count;
			}
		}

		static readonly ProfilerMarker m_DetermineCount = new(nameof(DetermineCount));

		int DetermineCount(int gridX, int gridY, int neighborX, int neighborY, int count,
			Span2D<int> original) {
			using (m_DetermineCount.Auto()) {
				if (IsWithinBoundary(neighborX, neighborY)) {
					if (neighborX != gridX || neighborY != gridY)
						count += original[neighborX, neighborY];
				}

				else {
					count++;
				}

				return count;
			}
		}

		static readonly ProfilerMarker m_IsWithinBoundary = new(nameof(IsWithinBoundary));

		bool IsWithinBoundary(int neighborX, int neighborY) {
			using (m_IsWithinBoundary.Auto()) {
				return neighborX > BORDER_SAFETY_FACTOR                       &&
				       neighborX < Ctx.Dimensions.Rows - BORDER_SAFETY_FACTOR &&
				       neighborY > BORDER_SAFETY_FACTOR                       &&
				       neighborY < Ctx.Dimensions.Columns - BORDER_SAFETY_FACTOR;
			}
		}

		internal StandardSmoothMapSolver(SmoothMapSolverCtx ctx) {
			Ctx = ctx;
		}

		const int BORDER_SAFETY_FACTOR = 4;
	}
}