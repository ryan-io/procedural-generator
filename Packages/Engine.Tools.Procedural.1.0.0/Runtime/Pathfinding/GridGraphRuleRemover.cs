using System.Collections.Generic;
using Pathfinding;
using UnityBCL;

namespace Engine.Procedural {
	public readonly struct GridGraphRuleRemover {
		public void Remove(GridGraph gridGraph) {
			var currentRules = new List<GridGraphRule>(gridGraph.rules.GetRules());

			if (!currentRules.IsEmptyOrNull())
				foreach (var rule in currentRules)
					gridGraph.rules.RemoveRule(rule);
		}
	}
}