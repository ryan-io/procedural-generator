// ProceduralGeneration

using System.Collections.Generic;

namespace ProceduralGeneration {
	internal readonly struct NavigationSolverCtx {
		internal TileHashset                        TileHashset       { get; }
		internal TileMapDictionary                  TileMapDictionary { get; }
		internal IReadOnlyList<GraphColliderCutter> ColliderCutters   { get; }

		internal NavigationSolverCtx(
			TileMapDictionary tileMapDictionary, 
			IReadOnlyList<GraphColliderCutter> colliderCutters, 
			TileHashset tileHashset) {
			TileMapDictionary = tileMapDictionary;
			ColliderCutters   = colliderCutters;
			TileHashset  = tileHashset;
		}
	}
}