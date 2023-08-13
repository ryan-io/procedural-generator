// ProceduralGeneration

using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	/// <summary>
	///  The meat of the generation process. This is where the map is actually generated.
	///  This is an unsafe method.
	///  Treat this as more of a script/ini file than a class.
	///  This is customizable; if you prefer to implement your algorithms, you can do so by inheriting from the
	///		appropriate solver class and defining a new process (inherit from GenerationProcess).
	/// </summary>
	internal class StandardProcess : GenerationProcess {
		internal override void Run(Span2D<int> map) {
			var ctxCreator = new ContextCreator(Actions);

			FillMap(map, ctxCreator.GetNewFillMapCtx());
			SmoothMap(map, ctxCreator.GetNewSmoothMapCtx());
			RemoveRegions(map, ctxCreator.GetNewRemoveRegionsCtx());
		}

		static void FillMap(Span2D<int> map, FillMapSolverCtx ctx) {
			ProceduralService.GetFillMapSolver(() => new CellularAutomataFillMapSolver(ctx))
			                 .Fill(map);
		}

		static void SmoothMap(Span2D<int> map, SmoothMapSolverCtx ctx) {
			ProceduralService.GetSmoothMapSolver(() => new MarchingSquaresSmoothMapSolver(ctx))
			                 .Smooth(map);
		}

		static void RemoveRegions(Span2D<int> map, RemoveRegionsSolverCtx ctx) {
			ProceduralService.GetRegionRemovalSolver(() => new FloodRegionRemovalSolver(ctx))
			                 .Remove(map);
		}

		public StandardProcess(IActions actions) : base(actions) {
		}
	}
}