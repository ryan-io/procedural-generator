using BCL;
using CommunityToolkit.HighPerformance;

namespace Engine.Procedural.Runtime {
	public class CellularAutomataFillMapSolver : FillMapSolver {
		StopWatchWrapper StopWatch          { get; }
		int              SeedHash           { get; }
		int              WallFillPercentage { get; }

		/// <summary>
		/// This method takes a Span and randomly changes value to '1' based on a WeightedRandom
		/// There is no need to stackalloc a copy of the span in this context
		/// </summary>
		/// <param name="primaryMap">Primary map span</param>
		public override void Fill(Span2D<int> primaryMap) {
			// rowsOrHeight = GetLength(0)
			// colsOrWidth = GetLength(1)
			// this is clearly opposite of what I thought
			// https://stackoverflow.com/questions/4260207/how-do-you-get-the-width-and-height-of-a-multi-dimensional-array
			var pseudoRandom = CreateRandom();
			var rowLength    = primaryMap.Height;
			var columnLength = primaryMap.Width;
			var startTime    = StopWatch.TimeElapsed;
			
			for (var i = 0; i < rowLength * columnLength; i++) {
				var row   = i / columnLength;
				var colum = i % columnLength;
				
				primaryMap[row, colum] = DetermineWallFill(rowLength, columnLength, row, colum, pseudoRandom);
			}
			
			var timeDelta = StopWatch.TimeElapsed - startTime;

			GenLogging.Instance.LogWithTimeStamp(
				LogLevel.Normal,
				StopWatch.TimeElapsed,
				"Total time to fill map: " + timeDelta,
				"FillMapSolver");
		}

		WeightedRandom<int> CreateRandom() {
			var items = new[] {
				new WeightedRandom<int>.Entry
					{ Item = 1, AccumulatedWeight = WallFillPercentage },
				new WeightedRandom<int>.Entry
					{ Item = 0, AccumulatedWeight = 100 - WallFillPercentage }
			};

			var pseudoRandom = new WeightedRandom<int>(SeedHash);
			pseudoRandom.AddRange(items);

			return pseudoRandom;
		}

		int DetermineWallFill(int rowLength, int columnLength, int x, int y, WeightedRandom<int> pseudoRandom)
			=> Utility.IsBoundary(rowLength, columnLength, x, y) ? 1 : pseudoRandom.Pop();

		public CellularAutomataFillMapSolver(ProceduralConfig model, StopWatchWrapper stopWatch) {
			SeedHash           = model.Seed.GetHashCode();
			WallFillPercentage = model.WallFillPercentage;
			StopWatch          = stopWatch;
		}
	}
}