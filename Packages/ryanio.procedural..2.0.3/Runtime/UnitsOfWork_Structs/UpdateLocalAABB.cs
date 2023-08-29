// ProceduralGeneration

using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	internal static class UpdateLocalAABB {
		internal static void Update(SpriteShapeRenderer renderer, SpriteShapeController controller, Transform tr) {
			UnityEngine.Bounds bounds = new UnityEngine.Bounds();

			for (var i = 0; i < controller.spline.GetPointCount(); i++) 
				bounds.Encapsulate(controller.spline.GetPosition(i));

			bounds.Encapsulate(tr.position);
			renderer.SetLocalAABB(bounds);
			controller.RefreshSpriteShape();
		}
	}
}