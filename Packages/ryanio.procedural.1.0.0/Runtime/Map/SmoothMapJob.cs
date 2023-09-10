// ProceduralGeneration

using System;
using JacksonDunstan.NativeCollections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace ProceduralGeneration {
	/*
	[BurstCompile]
	internal struct DetermineNeighborJob : IJobParallelFor {
		[ReadOnly] NativeArray<int> _mapRows;

		public void Execute(int index) {
			var surroundingWalls = GetAdjacentWallsCount(row, col, ref map, dimensions);

			if (surroundingWalls > COMPARISON_LIMIT)
				copy[row, col] = 1;

			else if (surroundingWalls < COMPARISON_LIMIT)
				copy[row, col] = 0;
		}

		public DetermineNeighborJob(ref int[,] map) {
			_map = map;
			_mapRows = new NativeArray<int>(map.GetLength(0), Allocator.Persistent);
		}

		readonly int[,] _map;
	}
	*/

	/// <summary>
	///   Will take the primary map span, make a copy, and perform calculations which is stored in MapProcessed
	/// </summary>
	[BurstCompile]
	internal struct SmoothMapJobCombined : IJob, IDisposable {
		public NativeArray2D<int> MapNativeModified;

		[ReadOnly] public NativeArray2D<int> MapNativeOriginal;
		[ReadOnly] public NativeArray<int> Rows;
		[ReadOnly] public NativeArray<int> Cols;
		
		public void Execute() {
			for (var row = 0; row < Rows[0]; row++) {
				for (var col = 0; col < Cols[0]; col++) {
					var surroundingWalls = GetAdjacentWallsCount(row, col);

					if (surroundingWalls > COMPARISON_LIMIT) {
						MapNativeModified[row, col] = 1;
					}

					else if (surroundingWalls < COMPARISON_LIMIT) {
						MapNativeModified[row, col] = 0;
					}
				}
			}
		}

		int GetAdjacentWallsCount(int row, int col) {
			var count = 0;

			for (var neighborRow = row - 1; neighborRow <= row + 1; neighborRow++) {
				for (var neighborCol = col - 1; neighborCol <= col + 1; neighborCol++)
					count = DetermineCount(row, col, neighborRow, neighborCol, count);
			}

			return count;
		}

		int DetermineCount(int row, int col, int neighborRow, int neighborCol, int count) {
			if (IsWithinBoundary(neighborRow, neighborCol)) {
				if (neighborRow != row || neighborCol != col) {
					count += MapNativeOriginal[row, col];
				}
			}

			else {
				count++;
			}

			return count;
		}

		bool IsWithinBoundary(int neighborX, int neighborY) {
			return neighborX > BORDER_SAFETY_FACTOR        &&
			       neighborX < Rows[0] - BORDER_SAFETY_FACTOR &&
			       neighborY > BORDER_SAFETY_FACTOR        &&
			       neighborY < Cols[0] - BORDER_SAFETY_FACTOR;
		}

		public void Dispose() {
			MapNativeModified.Dispose();
			MapNativeOriginal.Dispose();
		}

		const int COMPARISON_LIMIT     = 4;
		const int BORDER_SAFETY_FACTOR = 4;
	}
}