// ProceduralGeneration

using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///   Will take the primary map span, make a copy, and perform calculations which is stored in MapProcessed
	/// </summary>
	[BurstCompile]
	internal struct SmoothMapJobCombined : IJobFor, IDisposable {
		[ReadOnly] NativeArray<int> _mapRows;
		[ReadOnly] NativeArray<int> _mapCols;
		[ReadOnly] NativeArray<int> _output;

		[NativeDisableParallelForRestriction] public NativeArray<int> OutputProcessed;

		public void Execute(int index) {
			var rows = _mapRows.Length;
			var cols = _mapCols.Length;

			var tracker = 0;
			for (var row = 0; row < rows; row++) {
				for (var col = 0; col < cols; col++) {
					//DetermineNeighborLimits(row, col, original, copy, dimensions);
					var surroundingWalls = GetAdjacentWallsCount(row, col, tracker);

					if (surroundingWalls > COMPARISON_LIMIT) {
						OutputProcessed[tracker] = 1;
					}

					else if (surroundingWalls < COMPARISON_LIMIT) {
						OutputProcessed[tracker] = 0;
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
					count += _output[index];
				}
			}

			else {
				count++;
			}

			return count;
		}

		bool IsWithinBoundary(int neighborX, int neighborY) {
			var rows = _mapRows.Length;
			var cols = _mapCols.Length;

			return neighborX > BORDER_SAFETY_FACTOR        &&
			       neighborX < rows - BORDER_SAFETY_FACTOR &&
			       neighborY > BORDER_SAFETY_FACTOR        &&
			       neighborY < cols - BORDER_SAFETY_FACTOR;
		}
  
		public SmoothMapJobCombined(ref int[,] map) {
			_mapRows = new NativeArray<int>(map.GetLength(0),                    Allocator.Persistent);
			_mapCols = new NativeArray<int>(map.GetLength(1),                    Allocator.Persistent);
			_output  = new NativeArray<int>(map.GetLength(0) * map.GetLength(1), Allocator.Persistent);

			var outputIndex = 0;

			for (var row = 0; row < map.GetLength(0); row++) {
				_mapRows[row] = row;
				for (var col = 0; col < map.GetLength(1); col++) {
					_mapCols[col]        = col;
					_output[outputIndex] = map[row, col];
					outputIndex++;
				}
			}

			OutputProcessed = new NativeArray<int>(map.GetLength(0) * map.GetLength(1), Allocator.Persistent);
		}

		public void Dispose() {
			_mapRows.Dispose();
			_mapCols.Dispose();
			_output.Dispose();
			OutputProcessed.Dispose();
		}

		const int COMPARISON_LIMIT     = 4;
		const int BORDER_SAFETY_FACTOR = 4;
	}
}