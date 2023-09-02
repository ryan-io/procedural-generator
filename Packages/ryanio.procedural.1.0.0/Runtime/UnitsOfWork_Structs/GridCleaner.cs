// Engine.Procedural

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct GridPosition {
		internal static void Set(Dimensions dimensions, Grid grid, int borderSize) {
			if (grid == null)
				return;

			grid.transform.position =new Vector3(
				-dimensions.Rows    / 2f + borderSize / 2f,
				-dimensions.Columns / 2f + borderSize / 2f,
				0f);
		}
	}
}