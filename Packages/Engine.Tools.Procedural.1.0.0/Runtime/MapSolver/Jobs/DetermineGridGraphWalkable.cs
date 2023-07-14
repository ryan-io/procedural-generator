using Pathfinding;
using Pathfinding.Util;

namespace Engine.Procedural.Jobs {
	[Preserve]
	public class DetermineGridGraphWalkable : GridGraphRule {
		public DetermineGridGraphWalkable(GridGraphModel dto) {
		}

		public override void Register(GridGraphRules rules) {
		}

		void Rule(GridGraphRules.Context ctx) {
			ctx.graph.GetNodes(node => ctx.graph.CalculateConnections((GridNodeBase)node));
		}
	}
}