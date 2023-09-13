// Engine.Procedural

using Pathfinding;

namespace ProceduralGeneration {
	public class NavGraphRulesSolver {
		public void ResetGridGraphRules(GridGraph graph) {
			new GridGraphRuleRemover().Remove(graph);
		}

		public void SetGridGraphRules(GridGraph graph, ref int[,] map) {
			var walkabilityRule = new WalkabilityRule(ref map);
			graph.rules.AddRule(walkabilityRule);
		}

		public NavGraphRulesSolver() {
		}
	}
}