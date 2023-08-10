using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	public readonly struct TileMapCompressor {
		GameObject TileMapContainer { get; }
		
		public void Compress() {
			var tileMaps = TileMapContainer.GetComponentsInChildren<Tilemap>(true);

			foreach (var tilemap in tileMaps)
				if (tilemap)
					tilemap.CompressBounds();
		}

		public TileMapCompressor(GameObject tileMapContainer) {
			TileMapContainer = tileMapContainer;
		}
	}
}