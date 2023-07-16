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
		/// <param name="primarySpan">Primary map span</param>
		public override void Fill(Span2D<int> primarySpan) {
			var pseudoRandom = CreateRandom();

			var startTime = StopWatch.TimeElapsed;

			for (var x = 0; x < MapWidth; x++) {
				for (var y = 0; y < MapHeight; y++) {
					primarySpan[x, y] = DetermineWallFill(x, y, pseudoRandom);
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