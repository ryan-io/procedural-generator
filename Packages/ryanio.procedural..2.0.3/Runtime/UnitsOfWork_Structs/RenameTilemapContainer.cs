// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct RenameTilemapContainer {
		public void Rename(string name, GameObject container) {
			if (!container || string.IsNullOrWhiteSpace(name))
				return;

			container.name = Constants.SAVE_MAP_PREFIX + name;
		}
	}
}