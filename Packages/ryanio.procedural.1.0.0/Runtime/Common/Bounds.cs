using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	public static class Bounds {
		public static IDictionary<GameObject, float> GetBounds(GameObjectWeightTable objects) {
			var bounds = new PoissonObjects();
			foreach (var pair in objects) {
				var objRenderer = pair.Key.GetComponent<Renderer>();
				if (objRenderer == null) continue;

				var objBounds = objRenderer.bounds;
				var x         = objBounds.extents.x;
				var y         = objBounds.extents.y;

				bounds.Add(pair.Key, Math.Abs(x - y) < 0.001f ? x : Mathf.Max(x, y));
			}

			return bounds;
		}
	}
}