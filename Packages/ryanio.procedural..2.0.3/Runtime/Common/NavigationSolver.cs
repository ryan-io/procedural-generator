// ProceduralGeneration

using System.Collections.Generic;
using Pathfinding;

namespace ProceduralGeneration {
	internal class NavigationSolver {
		NavGraphBuilder<GridGraph>         Builder         { get; }
		NavGraphRulesSolver                Solver          { get; }
		GraphScanner                       Scanner         { get; }
		IReadOnlyList<GraphColliderCutter> ColliderCutters { get; }

		internal void Build() {
			var graph = Builder.Build();
			Solver.ResetGridGraphRules(graph);
			Solver.SetGridGraphRules(graph);
			Scanner.ScanGraph(graph);
			new CutGraphColliders().Cut(ColliderCutters);
		}

		public NavigationSolver(NavGraphBuilder<GridGraph> builder, NavigationSolverCtx ctx) {
			Builder         = builder;
			ColliderCutters = ctx.ColliderCutters;
			Solver          = new NavGraphRulesSolver(ctx.TileMapDictionary);
			Scanner         = new GraphScanner();
		}
	}
}