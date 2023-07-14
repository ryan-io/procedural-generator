﻿// Engine.Procedural

namespace Engine.Procedural {
	public readonly struct TilemapCleaner {
		public void Clean(ProceduralConfig config) {
			foreach (var tilemap in config.TileMapDictionary.Values) {
				if (!tilemap)
					continue;

				tilemap.ClearAllTiles();
			}
		}
	}
}