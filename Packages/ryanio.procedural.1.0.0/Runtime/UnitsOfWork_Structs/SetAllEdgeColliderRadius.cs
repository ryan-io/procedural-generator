// Engine.Procedural

using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct SetAllEdgeColliderRadius {
		float Radius { get; }

		public void Set(GameObject owner) {
			var col = owner.GetComponentsInChildren<EdgeCollider2D>();

			if (col.IsEmptyOrNull())
				return;

			foreach (var c in col) {
				if (c)
					c.edgeRadius = Radius;
			}
		}

		public SetAllEdgeColliderRadius(float radius) {
			Radius = radius;
		}
	}
}