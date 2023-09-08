using CommunityToolkit.HighPerformance;
using Unity.Burst;
using Unity.Profiling;

namespace ProceduralGeneration {
	internal class StandardSmoothMapSolver : SmoothMapSolver {
		int SmoothingIterations { get; }

		/// <summary>
		/// Will take the primary map span, make a copy, performed calculations over the entire mapSpanCopy
		/// The number of times to perform all calculations = SmoothingIterations
		/// Once each iteration is complete, it will copy all items in mapSpanCopy to mapSpan and repeat
		/// </summary>
		/// <param name="map">The primary map span</param>
		/// <param name="dimensions">Map dimensions</param>
		[BurstCompile]
		internal override void Smooth(Span2D<int> map, Dimensions dimensions) {
			var copy = new Span2D<int>(map.ToArray());

			for (var i = 0; i < SmoothingIterations; i++) {
				GetSmoothedMap(map, copy, dimensions);
				copy.CopyTo(map);
			}
			
			/*
			 * OPTIONAL -> this leads to more chaos
			 * for (var i = 0; i < Ctx.SmoothingIterations; i++) {
				GetSmoothedMap(map, copy);
				copy.CopyTo(map);
			}
			 */
		}

		static readonly ProfilerMarker m_GetSmoothedMap = new(nameof(GetSmoothedMap));

		static void GetSmoothedMap(Span2D<int> original, Span2D<int> copy, Dimensions dimensions) {
			using (m_GetSmoothedMap.Auto()) {
				var rows    = original.Height;
				var columns = original.Width;
				
				// for (int i = 0; i < rows * columns; i++) {
				// 	int x = i % columns, y = i / rows;
				// 	copy = DetermineNeighborLimits( x,  y, copy, original);
				// }
				
				
				for (var x = 0; x < rows; x++) {
					
					
					for (var y = 0; y < columns; y++) {
						DetermineNeighborLimits(x, y, original, copy, dimensions);
					}
				}

				//return copy;
			}
		}

		const int COMPARISON_LIMIT = 4;

		static readonly ProfilerMarker m_DetermineNeighborLimits = new(nameof(DetermineNeighborLimits));

		static void DetermineNeighborLimits(int x, int y, Span2D<int> original, Span2D<int> copy, Dimensions dimensions) {
			using (m_DetermineNeighborLimits.Auto()) {
				var surroundingWalls = GetAdjacentWallsCount(x, y, original, dimensions);

				if (surroundingWalls > COMPARISON_LIMIT)
					copy[x, y] = 1;

				else if (surroundingWalls < COMPARISON_LIMIT)
					copy[x, y] = 0;

				//return copy;
			}
		}

		static readonly ProfilerMarker m_GetAdjacentWallsCount = new(nameof(GetAdjacentWallsCount));

		static int GetAdjacentWallsCount(int x, int y, Span2D<int> original, Dimensions dimensions) {
			using (m_GetAdjacentWallsCount.Auto()) {
				var count = 0;

				for (var neighborX = x - 1; neighborX <= x + 1; neighborX++) {
					for (var neighborY = y - 1; neighborY <= y + 1; neighborY++) 
						count = DetermineCount(x, y, neighborX, neighborY, count, original, dimensions);
				}

				return count;
			}
		}

		static readonly ProfilerMarker m_DetermineCount = new(nameof(DetermineCount));

		static int DetermineCount(int gridX, int gridY, int neighborX, int neighborY, int count,
			Span2D<int> original, Dimensions dimensions) {
			using (m_DetermineCount.Auto()) {
				if (IsWithinBoundary(neighborX, neighborY, dimensions)) {
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

		static bool IsWithinBoundary(int neighborX, int neighborY, Dimensions dimensions) {
			using (m_IsWithinBoundary.Auto()) {
				return neighborX > BORDER_SAFETY_FACTOR                       &&
				       neighborX < dimensions.Rows - BORDER_SAFETY_FACTOR &&
				       neighborY > BORDER_SAFETY_FACTOR                       &&
				       neighborY < dimensions.Columns - BORDER_SAFETY_FACTOR;
			}
		}

		internal StandardSmoothMapSolver(SmoothMapSolverCtx ctx) {
			SmoothingIterations = ctx.SmoothingIterations;
		}

		const int BORDER_SAFETY_FACTOR = 4;
	}
}