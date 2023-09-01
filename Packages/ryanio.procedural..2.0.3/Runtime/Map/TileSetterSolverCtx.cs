// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct TileSolversCtx {
		internal Dimensions        Dimensions        { get; }
		internal TileMapDictionary TileMapDictionary { get; }
		internal TileDictionary    TileDictionary    { get; }
		internal TileHashset       TileHashset       { get; }
		internal Grid              Grid              { get; }
		internal bool              ShouldRenderTiles { get; }

		public TileSolversCtx(Dimensions dimensions, TileMapDictionary tileMapDictionary,
			TileDictionary tileDictionary, TileHashset tileHashset, Grid grid, bool shouldRenderTiles) {
			Dimensions        = dimensions;
			TileMapDictionary = tileMapDictionary;
			TileDictionary    = tileDictionary;
			TileHashset       = tileHashset;
			Grid              = grid;
			ShouldRenderTiles = shouldRenderTiles;
		}
	}
}