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
		/// <param name="mapSpan">The primary map span</param>
		public override unsafe void Smooth(Span2D<int> mapSpan) {
			NumberOfRows = mapSpan.Height;
			NumberOfCols = mapSpan.Width;
			
			Debug.Log("NumRows: " + NumberOfRows);
			Debug.Log("NumCols: " + NumberOfCols);
			int* copyToAllocationPointer = stackalloc int[3 * NumberOfRows * NumberOfCols];
			var  mapSpanCopy             = new Span2D<int>(copyToAllocationPointer, NumberOfRows, NumberOfCols, 0);
debug
			// for (var i = 0; i < NumberOfRows * NumberOfCols; i++) {
			// 	var row   = i / NumberOfCols;
			// 	var colum = i % NumberOfCols;
			//
			// 	mapSpanCopy[row, colum] = mapSpan[row, colum];
			// }
			mapSpan.CopyTo(mapSpanCopy);
			// SmoothingIterations is relatively small in all scenarios
			for (var i = 0; i < SmoothingIterations; i++) {
				//	mapSpan.CopyTo(mapSpanCopy);
				//var hashTable = GetNewHashTable(mapSpanCopy, hash);
				
				GetSmoothedMap(mapSpan, mapSpan);
				
				for (var j = 0; i < NumberOfRows * NumberOfCols; j++) {
					var row   = j / NumberOfCols;
					var colum = j % NumberOfCols;

					mapSpan[row, colum] = mapSpanCopy[row, colum];
				}
			}
		}

		void GetSmoothedMap(Span2D<int> mapSpanOriginal, Span2D<int> mapSpanCopy) {
			for (var i = 0; i < NumberOfRows * NumberOfCols; i++) {
				var row   = i / NumberOfCols;
				var colum = i % NumberOfCols;

				DetermineNeighborLimits(row, colum, mapSpanCopy, mapSpanOriginal);
			}


			//
			// for (var x = 0; x < NumberOfRows; x++) {
			// 	for (var y = 0; y < NumberOfCols; y++) {
			// 		DetermineNeighborLimits(x, y, mapSpanCopy, mapSpanOriginal);
			// 	}
			// }
		}

		void DetermineNeighborLimits(int x, int y, Span2D<int> mapSpanCopy, Span2D<int> mapSpanOriginal) {
			var surroundingWalls = GetAdjacentWallsCount(x, y, mapSpanOriginal);

			if (surroundingWalls > UpperNeighborLimit)
				mapSpanCopy[x, y] = 1;

			else if (surroundingWalls < LowerNeighborLimit)
				mapSpanCopy[x, y] = 0;
		}

		int GetAdjacentWallsCount(int x, int y, Span2D<int> mapSpanOriginal) {
			var count = 0;

			for (var neighborX = x - 1; neighborX <= x + 1; neighborX++) {
				for (var neighborY = y - 1; neighborY <= y + 1; neighborY++)
					count = DetermineCount(x, y, neighborX, neighborY, count, mapSpanOriginal);
			}

			return count;
		}

		int DetermineCount(int gridX, int gridY, int neighborX, int neighborY, int count, Span2D<int> mapSpanOriginal) {
			if (IsWithinBoundary(neighborX, neighborY)) {
				if (neighborX != gridX || neighborY != gridY)
					count += mapSpanOriginal[neighborX, neighborY];
			}

			else {
				count++;
			}

			return count;
		}

		bool IsWithinBoundary(int neighborX, int neighborY)
			=> neighborX >= 0 && neighborX < NumberOfRows && neighborY >= 0 && neighborY < NumberOfCols;

		public MarchingSquaresSmoothMapSolver(ProceduralConfig config, StopWatchWrapper stopWatch) {
			UpperNeighborLimit  = config.UpperNeighborLimit;
			LowerNeighborLimit  = config.LowerNeighborLimit;
			SmoothingIterations = config.SmoothingIterations;
			StopWatch           = stopWatch;
		}
	}
}