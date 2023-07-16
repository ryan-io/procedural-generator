using BCL;
using CommunityToolkit.HighPerformance;

namespace Engine.Procedural {
	public class CellularAutomataFillMapSolver : FillMapSolver {
		StopWatchWrapper StopWatch          { get; }
		int              SeedHash           { get; }
		int              MapWidth           { get; }
		int              MapHeight          { get; }
		int              WallFillPercentage { get; }

		/// <summary>
		/// This method takes a Span and randomly changes value to '1' based on a WeightedRandom
		/// There is no need to stackalloc a copy of the span in this context
		/// </summary>
		/// <param name="primaryMap">Primary map span</param>
		public override unsafe void Fill(Span2D<int> primaryMap) {
			// rowsOrHeight = GetLength(0)
			// colsOrWidth = GetLength(1)
			// this is clearly opposite of what I thought
			// https://stackoverflow.com/questions/4260207/how-do-you-get-the-width-and-height-of-a-multi-dimensional-array
			var pseudoRandom   = CreateRandom();

			var startTime = StopWatch.TimeElapsed;

			for (var x = 0; x < primaryMap.Height; x++) {
				for (var y = 0; y < primaryMap.Width; y++) {
					primaryMap[x, y] = DetermineWallFill(x, y, pseudoRandom);
				}
			}

			var timeDelta = StopWatch.TimeElapsed - startTime;

			GenLogging.LogWithTimeStamp(
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

		int DetermineWallFill(int x, int y, WeightedRandom<int> pseudoRandom)
			=> Utility.IsBoundary(MapWidth, MapHeight, x, y) ? 1 : pseudoRandom.Pop();

		public CellularAutomataFillMapSolver(ProceduralConfig model, StopWatchWrapper stopWatch) {
			SeedHash           = model.Seed.GetHashCode();
			MapWidth           = model.Width;
			MapHeight          = model.Height;
			WallFillPercentage = model.WallFillPercentage;
			StopWatch          = stopWatch;
		}
	}
}