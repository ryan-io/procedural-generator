// ProceduralGeneration

namespace ProceduralGeneration {
	internal readonly struct NavigationSolverCtx {
		internal TileMapDictionary TileMapDictionary { get; }

		internal NavigationSolverCtx(TileMapDictionary tileMapDictionary) {
			TileMapDictionary = tileMapDictionary;
		}
	}
}