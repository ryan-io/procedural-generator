using BCL;

namespace ProceduralGeneration {
	internal class CellularAutomataFillMapSolver : FillMapSolver {
		FillMapSolverCtx   Ctx    { get; }
		
		/// <summary>
		/// This method takes a Span and randomly changes value to '1' based on a WeightedRandom
		/// There is no need to stackalloc a copy of the span in this context
		/// </summary>
		/// <param name="map">Primary map span</param>
		internal override void Fill(ref int[,] map) {
			// rowsOrHeight = GetLength(0)
			// colsOrWidth = GetLength(1)
			// this is clearly opposite of what I thought
			// https://stackoverflow.com/questions/4260207/how-do-you-get-the-width-and-height-of-a-multi-dimensional-array
			var pseudoRandom = CreateRandom(Ctx.WallFillPercentage);
			var rowLength    = map.GetLength(0);
			var columnLength = map.GetLength(1);
			
			for (var x = 0; x < rowLength; x++) {
				for (var y = 0; y < columnLength; y++) {
					map[x, y] = DetermineWallFill(rowLength, columnLength, x, y, pseudoRandom);
				}
			}
		}

		WeightedRandom<int> CreateRandom(int wallFillPercentage) {
			var items = new[] {
				new WeightedRandom<int>.Entry
					{ Item = 1, AccumulatedWeight = wallFillPercentage },
				new WeightedRandom<int>.Entry
					{ Item = 0, AccumulatedWeight = 100 - wallFillPercentage }
			};

			var pseudoRandom = new WeightedRandom<int>(Ctx.SeedHash);
			pseudoRandom.AddRange(items);

			return pseudoRandom;
		}

		int DetermineWallFill(int rowLength, int columnLength, int x, int y, WeightedRandom<int> pseudoRandom)
			=> Utility.IsBoundary(rowLength, columnLength, x, y) ? 1 : pseudoRandom.Pop();

		public CellularAutomataFillMapSolver(FillMapSolverCtx ctx) {
			Ctx = ctx;
		}
	}
}