// ProceduralGeneration

using System.Collections.Generic;
using System.Threading;
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
			Scanner.Fire(new GraphScanner.Args(graph, false), CancellationToken.None);
			new CutGraphColliders().Cut(ColliderCutters);
		}

		public NavigationSolver(NavGraphBuilder<GridGraph> builder, NavigationSolverCtx ctx) {
			Builder         = builder;
			ColliderCutters = ctx.ColliderCutters;
			Solver          = new NavGraphRulesSolver(ctx.TileMapDictionary, ctx.TileHashset);
			Scanner         = new GraphScanner();
		}
	}
}