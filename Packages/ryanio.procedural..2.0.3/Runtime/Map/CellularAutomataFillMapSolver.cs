using BCL;
using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal class CellularAutomataFillMapSolver : FillMapSolver {
		FillMapSolverCtx   Ctx    { get; }
		
		/// <summary>
		/// This method takes a Span and randomly changes value to '1' based on a WeightedRandom
		/// There is no need to stackalloc a copy of the span in this context
		/// </summary>
		/// <param name="primaryMap">Primary map span</param>
		internal override void Fill(Span2D<int> primaryMap) {
			// rowsOrHeight = GetLength(0)
			// colsOrWidth = GetLength(1)
			// this is clearly opposite of what I thought
			// https://stackoverflow.com/questions/4260207/how-do-you-get-the-width-and-height-of-a-multi-dimensional-array
			var pseudoRandom = CreateRandom(Ctx.WallFillPercentage);
			var rowLength    = primaryMap.Height;
			var columnLength = primaryMap.Width;
			
			for (var i = 0; i < rowLength * columnLength; i++) {
				var row   = i / columnLength;
				var colum = i % columnLength;
				
				primaryMap[row, colum] = DetermineWallFill(rowLength, columnLength, row, colum, pseudoRandom);
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