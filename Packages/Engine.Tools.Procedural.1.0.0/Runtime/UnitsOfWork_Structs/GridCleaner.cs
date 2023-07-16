// Engine.Procedural

using UnityEngine;

namespace Engine.Procedural {
	public readonly struct GridCleaner {
		public void Clean(ProceduralConfig config) {
			config.Grid.transform.position = new Vector3(
				-config.Width  / 2f + config.BorderSize / 2f,
				-config.Height / 2f + config.BorderSize / 2f,
				0f);
		}
	}
}