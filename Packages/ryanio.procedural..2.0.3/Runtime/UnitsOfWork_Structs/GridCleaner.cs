// Engine.Procedural

using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct SetGridPosition {
		public void Clean(ProceduralConfig config, Grid grid) {
			if (grid == null)
				return;
			
			grid.transform.position = new Vector3(
				-config.Rows  / 2f + config.BorderSize / 2f,
				-config.Columns / 2f + config.BorderSize / 2f,
				0f);
		}
	}
}