// ProceduralGeneration

using System.Collections.Generic;

namespace ProceduralGeneration {
	internal readonly struct NavigationSolverCtx {
		internal TileMapDictionary TileMapDictionary { get; }
		internal IReadOnlyList<GraphColliderCutter> ColliderCutters  { get; }

		internal NavigationSolverCtx(TileMapDictionary tileMapDictionary, IReadOnlyList<GraphColliderCutter> colliderCutters) {
			TileMapDictionary    = tileMapDictionary;
			ColliderCutters = colliderCutters;
		}
	}
}