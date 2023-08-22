// ProceduralGeneration

using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	internal class SpriteShapeRefreshService {
		GameObject Owner { get; }

		internal void Run() {
			if (!Owner) return;
			
			var controllers = Owner.GetComponentsInChildren<SpriteShapeController>();

			if (controllers.IsEmptyOrNull())
				return;

			foreach (var con in controllers) {
				if (!con)
					continue;

				con.UpdateSpriteShapeParameters();
				con.RefreshSpriteShape();
			}
		}
		
		public SpriteShapeRefreshService(GameObject gameObject) {
			Owner = gameObject;
		}
	}
}