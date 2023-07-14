using UnityEngine;

namespace Engine.Procedural {
	public static class LayerHelper {
		public static void GameObjectToLayer(GameObject o, string layerName) {
			if (!o || string.IsNullOrWhiteSpace(layerName))
				return;

			o.layer = LayerMask.NameToLayer(layerName);
		}

		public static bool IsNotAnObstacleLayer(string name) {
			if (string.IsNullOrWhiteSpace(name))
				return false;

			return LayerMask.NameToLayer(name) == -1;
		}
	}
}