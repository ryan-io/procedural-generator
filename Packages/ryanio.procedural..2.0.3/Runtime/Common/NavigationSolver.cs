// ProceduralGeneration

using Pathfinding;

namespace ProceduralGeneration {
	internal class NavigationSolver {
		NavGraphBuilder<GridGraph> Builder { get; }
		NavGraphRulesSolver        Solver  { get; }
		GraphScanner               Scanner { get; }

		internal void Build() {
			var graph = Builder.Build();
			Solver.ResetGridGraphRules(graph);
			Solver.SetGridGraphRules(graph);
			Scanner.ScanGraph(graph);
		}

		public NavigationSolver(NavGraphBuilder<GridGraph> builder, NavigationSolverCtx ctx) {
			Builder = builder;
			Solver  = new NavGraphRulesSolver(ctx.TileMapDictionary);
			Scanner = new GraphScanner();
		}
	}
}