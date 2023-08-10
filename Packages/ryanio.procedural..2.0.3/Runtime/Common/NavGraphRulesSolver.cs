// Engine.Procedural

using Pathfinding;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
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

		public NavGraphRulesSolver(TileMapDictionary dictionary) {
			GroundTilemap   = dictionary[TileMapType.Ground];
			BoundaryTilemap = dictionary[TileMapType.Boundary];
		}
	}
}