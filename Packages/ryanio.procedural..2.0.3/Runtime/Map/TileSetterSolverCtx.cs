// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct TileSolversCtx {
		internal Dimensions        Dimensions        { get; }
		internal TileMapDictionary TileMapDictionary { get; }
		internal TileDictionary    TileDictionary    { get; }
		internal TileHashset       TileHashset       { get; }
		internal Grid              Grid              { get; }

		public TileSolversCtx(Dimensions dimensions, TileMapDictionary tileMapDictionary,
			TileDictionary tileDictionary, TileHashset tileHashset, Grid grid) {
			Dimensions        = dimensions;
			TileMapDictionary = tileMapDictionary;
			TileDictionary    = tileDictionary;
			TileHashset       = tileHashset;
			Grid         = grid;
		}
	}
}