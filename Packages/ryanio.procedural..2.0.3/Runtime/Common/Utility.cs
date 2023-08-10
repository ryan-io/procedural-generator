using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	public static class Utility {
		public static bool IsBoundary(int mapWidth, int mapHeight, int x, int y) =>
			x <= 0             ||
			y <= 0             ||
			x == mapWidth  - 1 ||
			y == mapHeight - 1;

		public static bool HasTileAtPosition(Tilemap tilemap, Vector3Int position) => tilemap.HasTile(position);

		public static Vector3 GetTileWorldPosition(Tilemap tilemap, GridLayout layout, Vector3Int tileMapPosition) {
			if (!tilemap || !layout)
				return default;

			var cellPosition = layout.CellToWorld(tileMapPosition);

			return cellPosition;
		}
	}
}