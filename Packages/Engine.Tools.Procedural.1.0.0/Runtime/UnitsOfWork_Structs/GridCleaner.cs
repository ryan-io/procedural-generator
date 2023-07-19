// Engine.Procedural

using UnityEngine;

namespace Engine.Procedural {
	public readonly struct GridCleaner {
		public void Clean(ProceduralConfig config) {
			config.Grid.transform.position = new Vector3(
				-config.Rows  / 2f + config.BorderSize / 2f,
				-config.Columns / 2f + config.BorderSize / 2f,
				0f);
		}
	}
}