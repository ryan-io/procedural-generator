using BCL;
using CommunityToolkit.HighPerformance;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class MarchingSquaresSmoothMapSolver : SmoothMapSolver {
		StopWatchWrapper StopWatch           { get; }
		int              UpperNeighborLimit  { get; }
		int              LowerNeighborLimit  { get; }
		int              SmoothingIterations { get; }
		int              NumberOfRows        { get; set; }
		int              NumberOfCols        { get; set; }

		/// <summary>
		/// Will take the primary map span, make a copy, performed calculations over the entire mapSpanCopy
		/// The number of times to perform all calculations = SmoothingIterations
		/// Once each iteration is complete, it will copy all items in mapSpanCopy to mapSpan and repeat
		/// </summary>
		/// <param name="original">The primary map span</param>
		public override void Smooth(int[,] original) {
			// int* copyToAllocationPointer = stackalloc int[3 * NumberOfRows * NumberOfCols];
			// var  mapSpanCopy             = new Span2D<int>(copyToAllocationPointer, NumberOfRows, NumberOfCols, 0);

			// for (var i = 0; i < NumberOfRows * NumberOfCols; i++) {
			// 	var row   = i / NumberOfCols;
			// 	var colum = i % NumberOfCols;
			//
			// 	mapSpanCopy[row, colum] = mapSpan[row, colum];
			// }

			var copy = (int[,])original.Clone();

			// SmoothingIterations is relatively small in all scenarios
			for (var i = 0; i < SmoothingIterations; i++) {
				//	mapSpan.CopyTo(mapSpanCopy);
				//var hashTable = GetNewHashTable(mapSpanCopy, hash);

				original = GetSmoothedMap(original, copy);
			}
		}

		int[,] GetSmoothedMap(int[,] original, int[,] copy) {
			for (var i = 0; i < NumberOfRows * NumberOfCols; i++) {
				var row   = i / NumberOfCols;
				var colum = i % NumberOfCols;

				copy = DetermineNeighborLimits(row, colum, copy, original);
			}

			//
			// for (var x = 0; x < NumberOfRows; x++) {
			// 	for (var y = 0; y < NumberOfCols; y++) {
			// 		DetermineNeighborLimits(x, y, mapSpanCopy, mapSpanOriginal);
			// 	}
			// }

			return copy;
		}

		int[,] DetermineNeighborLimits(int x, int y, int[,] original, int[,] copy) {
			var surroundingWalls = GetAdjacentWallsCount(x, y, original);

			if (surroundingWalls > UpperNeighborLimit)
				copy[x, y] = 1;

			else if (surroundingWalls < LowerNeighborLimit)
				copy[x, y] = 0;

			return copy;
		}

		int GetAdjacentWallsCount(int x, int y, int[,] original) {
			var count = 0;

			for (var neighborX = x - 1; neighborX <= x + 1; neighborX++) {
				for (var neighborY = y - 1; neighborY <= y + 1; neighborY++)
					count = DetermineCount(x, y, neighborX, neighborY, count, original);
			}

			return count;
		}

		int DetermineCount(int gridX, int gridY, int neighborX, int neighborY, int count, int[,] original) {
			if (IsWithinBoundary(neighborX, neighborY)) {
				if (neighborX != gridX || neighborY != gridY)
					count += original[neighborX, neighborY];
			}

			else {
				count++;
			}

			return count;
		}

		bool IsWithinBoundary(int neighborX, int neighborY)
			=> neighborX >= 0 && neighborX < NumberOfRows && neighborY >= 0 && neighborY < NumberOfCols;

		public MarchingSquaresSmoothMapSolver(ProceduralConfig config, StopWatchWrapper stopWatch) {
			NumberOfCols        = config.Columns;
			NumberOfRows        = config.Rows;
			UpperNeighborLimit  = config.UpperNeighborLimit;
			LowerNeighborLimit  = config.LowerNeighborLimit;
			SmoothingIterations = config.SmoothingIterations;
			StopWatch           = stopWatch;
		}
	}
}