using BCL;
using CommunityToolkit.HighPerformance;

namespace Engine.Procedural {
	public class MarchingSquaresSmoothMapSolver : SmoothMapSolver {
		StopWatchWrapper StopWatch           { get; }
		int              MapWidth            { get; }
		int              MapHeight           { get; }
		int              UpperNeighborLimit  { get; }
		int              LowerNeighborLimit  { get; }
		int              SmoothingIterations { get; }

		/// <summary>
		/// ** Research on optimizing O(n^2) to O(n) should be researched (hashtable)
		/// ** I'm not sure constructing a hash table will be of any benefit when working with Spans on the stack **
		///
		/// Will take the primary map span, make a copy, performed calculations over the entire mapSpanCopy
		/// The number of times to perform all calculations = SmoothingIterations
		/// Once each iteration is complete, it will copy all items in mapSpanCopy to mapSpan and repeat
		/// </summary>
		/// <param name="mapSpan">The primary map span</param>
		public override unsafe void Smooth(Span2D<int> mapSpan) {
			int* copyToAllocationPointer = stackalloc int[MapWidth * MapHeight];
			var  mapSpanCopy             = new Span2D<int>(copyToAllocationPointer, MapHeight, MapWidth, 0);
			mapSpan.CopyTo(mapSpanCopy);

			//IDictionary<ValueTuple<int, int>, int> hash = new Dictionary<(int, int), int>(new ValueTupleIntComparer());

			// SmoothingIterations is relatively small in all scenarios
			for (var i = 0; i < SmoothingIterations; i++) {
				//var hashTable = GetNewHashTable(mapSpanCopy, hash);
				GetSmoothedMap(mapSpan, mapSpanCopy);
				mapSpanCopy.CopyTo(mapSpan);
			}
		}

		// IDictionary<ValueTuple<int, int>, int> GetNewHashTable(Span2D<int> span,
		// 	IDictionary<ValueTuple<int, int>, int> currentHash) {
		// 	currentHash.Clear();
		//
		// 	var row = span.GetRow(1);
		//
		// 	foreach (var item in row) {
		// 		
		// 	}
		// 	
		// 	foreach (var element in span) {
		// 		var valueT = new ValueTuple<int, int>(2, 0);
		// 		
		// 	}
		// }

		void GetSmoothedMap(Span2D<int> mapSpanOriginal, Span2D<int> mapSpanCopy) {
			for (var x = 0; x < MapWidth; x++) {
				for (var y = 0; y < MapHeight; y++) {
					DetermineNeighborLimits(x, y, mapSpanCopy, mapSpanOriginal);
				}
			}
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
			=> neighborX >= 0 && neighborX < MapWidth && neighborY >= 0 && neighborY < MapHeight;

		public MarchingSquaresSmoothMapSolver(ProceduralConfig config, StopWatchWrapper stopWatch) {
			MapWidth            = config.Width;
			MapHeight           = config.Height;
			UpperNeighborLimit  = config.UpperNeighborLimit;
			LowerNeighborLimit  = config.LowerNeighborLimit;
			SmoothingIterations = config.SmoothingIterations;
			StopWatch           = stopWatch;
		}
	}
}