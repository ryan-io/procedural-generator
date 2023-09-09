// ProceduralGeneration

using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Vector3 = System.Numerics.Vector3;

namespace ProceduralGeneration {
	/// <summary>
	///   Will take the primary map span, make a copy, and perform calculations which is stored in MapProcessed
	/// </summary>
	[BurstCompile]
	internal struct SmoothMapJob : IJobFor, IDisposable {
		// x-component: row
		// y-component: column
		// z-component: value
		[ReadOnly] public NativeArray<Vector3> Map;
		[NativeDisableParallelForRestriction]
		public            NativeArray<Vector3> MapProcessed;

		// used in IJobParallelFor; this is to allow read access to indices in different threads
		//[NativeDisableParallelForRestriction]
		//public NativeArray<int> MapProcessedInt;

		// x-component: rowLength
		// y-component: columnLength
		// z-component: totalLength
		[NativeDisableParallelForRestriction]
		NativeArray<Vector3> m_Dimensions;

		public void Execute(int index) {
			var cols        = (int)m_Dimensions[0].Y;
			var rows        = (int)m_Dimensions[0].X;
            
			var tracker = 0;
			for (var row = 0; row < rows; row++) {
				for (var col = 0; col < cols; col++) {
					//DetermineNeighborLimits(row, col, original, copy, dimensions);
					var surroundingWalls = GetAdjacentWallsCount(row, col, tracker);

					if (surroundingWalls > COMPARISON_LIMIT) {
						MapProcessed[tracker] = new Vector3(row, col, 1);
					}

					else if (surroundingWalls < COMPARISON_LIMIT) {
						MapProcessed[tracker] = new Vector3(row, col, 0);
					}

					tracker++;
				}
			}
		}

		int GetAdjacentWallsCount(int row, int col, int index) {
			var count = 0;

			for (var neighborRow = row - 1; neighborRow <= row + 1; neighborRow++) {
				for (var neighborCol = col - 1; neighborCol <= col + 1; neighborCol++)
					count = DetermineCount(row, col, neighborRow, neighborCol, count, index);
			}

			return count;
		}

		int DetermineCount(int row, int col, int neighborRow, int neighborCol, int count, int index) {
			if (IsWithinBoundary(neighborRow, neighborCol)) {
				if (neighborRow != row || neighborCol != col) {
					count += (int)Map[index].Z;
				}
			}

			else {
				count++;
			}

			return count;
		}

		bool IsWithinBoundary(int neighborX, int neighborY) {
			var rows = (int)m_Dimensions[0].X;
			var cols = (int)m_Dimensions[0].Y;

			return neighborX > BORDER_SAFETY_FACTOR        &&
			       neighborX < rows - BORDER_SAFETY_FACTOR &&
			       neighborY > BORDER_SAFETY_FACTOR        &&
			       neighborY < cols - BORDER_SAFETY_FACTOR;
		}

		public SmoothMapJob(int[,] map) {
			var totalLength = map.Length;

			Map          = new NativeArray<Vector3>(totalLength, Allocator.Persistent);
			MapProcessed = new NativeArray<Vector3>(totalLength, Allocator.Persistent);
			int tracker = 0;
			
			for (var row = 0; row < map.GetLength(0); row++) {
				for (var col = 0; col < map.GetLength(1); col++) {
					Map[tracker] = new Vector3(row, col, map[row, col]);
					tracker++;
				}
			}

			m_Dimensions = new NativeArray<Vector3>(1, Allocator.Persistent) {
				[0] = new(map.GetLength(0), map.GetLength(1), totalLength)
			};
		}

		public void Dispose() {
			Map.Dispose();
			MapProcessed.Dispose();
			m_Dimensions.Dispose();
		}

		const int COMPARISON_LIMIT     = 4;
		const int BORDER_SAFETY_FACTOR = 4;
	}
}