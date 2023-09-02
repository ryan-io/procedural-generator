using ProceduralGeneration;
using UnityEditor;
using UnityEngine;

namespace Engine.Procedural.Editor.Editor {
	public class MenuItems {
		[MenuItem("Procedural Generation/Utility/Remove Tile Labels")]
		public static void RemoveLabels() {
			var objects = GameObject.FindGameObjectsWithTag(Constants.TILE_LABEL);
			var length  = objects.Length;

			for (var i = 0; i < length; i++)
				if (Application.isPlaying)
					Object.Destroy(objects[i]);

				else
					Object.DestroyImmediate(objects[i]);
		}
/*
		void RemoveTiles(ProceduralTileSceneObjects tileSceneObjects, int2 mapSize) {
			var shouldEraseAll = true;


			if (!Application.isPlaying)
				shouldEraseAll = EditorUtility.DisplayDialog(
					"Reset All", "Reset All Procedural Generation?", "Yes",
					"No");

			foreach (var map in tileSceneObjects.TileMapTable) {
				var shouldRemove = true;
				var sm           = _monoModel.ProceduralMapStateMachine;

				if (!shouldEraseAll && sm         != null                    &&
				    sm.ApplicationSm.CurrentState == ApplicationState.Editor && Application.isPlaying)
					shouldRemove = EditorUtility.DisplayDialog(
						"Reset Tiles",
						$"Do you want to remove all tiles on the {map.Key} map?",
						"Yes", "No");


				if (!shouldRemove)
					continue;

				var mapSizeX = mapSize.x;
				var mapSizeY = mapSize.y;

				for (var x = 0; x < mapSizeX; x++) {
					for (var y = 0; y < mapSizeY; y++)
						Utility.SetTileNullAtXY(x, y, map);
				}
			}
		}*/
	}
}