using Unity.Profiling;

namespace ProceduralGeneration {
	internal class StandardSmoothMapSolver : SmoothMapSolver {
		int                            SmoothingIterations { get; }
		static readonly ProfilerMarker m_Smooth = new(nameof(Smooth));
		
		// NativeArray2D<int> NativeMap { get; set; }
		// NativeArray2D<int> NativeMap { get; set; }

		/// <summary>
		/// Will take the primary map span, make a copy, performed calculations over the entire mapSpanCopy
		/// The number of times to perform all calculations = SmoothingIterations
		/// Once each iteration is complete, it will copy all items in mapSpanCopy to mapSpan and repeat
		/// </summary>
		/// <param name="map">The primary map span</param>
		/// <param name="dimensions">Map dimensions</param>
		internal override void Smooth(ref int[,] map, Dimensions dimensions) {
			using (m_Smooth.Auto()) {
				//NativeMap = new NativeArray2D<int>(map, Allocator.Persistent);
				
				var copy = map.Clone() as int[,]; // this is ok; value types are deep copied
				for (var i = 0; i < SmoothingIterations; i++) {
					GetSmoothedMap(ref map, ref copy, dimensions);
				
					for (var j = 0; j < map.GetLength(0); j++) {
						for (var k = 0; k < map.GetLength(1); k++) {
							map[j, k] = copy[j, k];
						}
					}
				}

				/*
				 * OPTIONAL -> this leads to more chaos
				 * for (var i = 0; i < Ctx.SmoothingIterations; i++) {
					GetSmoothedMap(map, copy);
					copy.CopyTo(map);
				}
				 */
			}
		}

		static readonly ProfilerMarker m_GetSmoothedMap = new(nameof(GetSmoothedMap));

		void GetSmoothedMap(ref int[,] map, ref int[,] copy, Dimensions dimensions) {
			using (m_GetSmoothedMap.Auto()) {
				var rows = map.GetLength(0);
				var cols = map.GetLength(1);

				for (var x = 0; x < rows; x++) {
					for (var y = 0; y < cols; y++) {
						DetermineNeighborLimits(x, y, ref map, ref copy, dimensions);
					}
				}
			}
		}

		const int COMPARISON_LIMIT = 4;

		static readonly ProfilerMarker m_DetermineNeighborLimits = new(nameof(DetermineNeighborLimits));

		void DetermineNeighborLimits(int row, int col, ref int[,] map, ref int[,] copy,
			Dimensions dimensions) {
			using (m_DetermineNeighborLimits.Auto()) {
				var surroundingWalls = GetAdjacentWallsCount(row, col, ref map, dimensions);

				if (surroundingWalls > COMPARISON_LIMIT)
					copy[row, col] = 1;

				else if (surroundingWalls < COMPARISON_LIMIT)
					copy[row, col] = 0;

				//return copy;
			}
		}

		static readonly ProfilerMarker m_GetAdjacentWallsCount = new(nameof(GetAdjacentWallsCount));

		int GetAdjacentWallsCount(int row, int col, ref int[,] original, Dimensions dimensions) {
			using (m_GetAdjacentWallsCount.Auto()) {
				var count = 0;

				for (var neighborX = row - 1; neighborX <= row + 1; neighborX++) {
					for (var neighborY = col - 1; neighborY <= col + 1; neighborY++) {
						
						count = DetermineCount(row, col, neighborX, neighborY, count, ref original, dimensions);
					}
				}

				return count;
			}
		}

		static readonly ProfilerMarker m_DetermineCount = new(nameof(DetermineCount));

		int DetermineCount(int gridX, int gridY, int neighborX, int neighborY, int count,
			ref int[,] original, Dimensions dimensions) {
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
				return neighborX > BORDER_SAFETY_FACTOR                   &&
				       neighborX < dimensions.Rows - BORDER_SAFETY_FACTOR &&
				       neighborY > BORDER_SAFETY_FACTOR                   &&
				       neighborY < dimensions.Columns - BORDER_SAFETY_FACTOR;
			}
		}

		internal StandardSmoothMapSolver(SmoothMapSolverCtx ctx) {
			SmoothingIterations = ctx.SmoothingIterations;
		}

		const int BORDER_SAFETY_FACTOR = 4;
	}
}