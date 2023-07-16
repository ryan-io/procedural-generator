// Engine.Procedural

using Pathfinding;
using UnityEngine.Tilemaps;

namespace Engine.Procedural {
	public class NavGraphRulesSolver {
		Tilemap GroundTilemap   { get; }
		Tilemap BoundaryTilemap { get; }

		public void ResetGridGraphRules(GridGraph graph) {
			new GridGraphRuleRemover().Remove(graph);
		}

		public void SetGridGraphRules(GridGraph graph) {
			var walkabilityRule = new WalkabilityRule(BoundaryTilemap, GroundTilemap, Constants.CELL_SIZE);
			graph.rules.AddRule(walkabilityRule);
		}

		public NavGraphRulesSolver(ProceduralConfig config) {
			GroundTilemap   = config.TileMapDictionary[TileMapType.Ground];
			BoundaryTilemap = config.TileMapDictionary[TileMapType.Boundary];
		}
	}
}