using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Engine.Procedural {
	public static class Utility {
		public static bool IsBoundary(int mapWidth, int mapHeight, int x, int y) =>
			x == 0 || y == 0 || x == mapWidth - 1 || y == mapHeight - 1;

		public static bool HasTileAtPosition(Tilemap tilemap, Vector3Int position) => tilemap.HasTile(position);

		public static Vector3 GetTileWorldPosition(Tilemap tilemap, GridLayout layout, Vector3Int tileMapPosition) {
			if (!tilemap || !layout)
				return default;

			var cellPosition = layout.CellToWorld(tileMapPosition);

			return cellPosition;
		}

#if UNITY_EDITOR
		public static void ClearLogs() {
			var assembly = Assembly.GetAssembly(typeof(Editor));
			var type = assembly.GetType("UnityEditor.LogEntries");
			var method = type.GetMethod("Clear");
			method?.Invoke(new object(), null);
		}

#endif
	}
}