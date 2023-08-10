// ProceduralGeneration

using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	public readonly struct CleanSpriteShapes {
		public void Clean(GameObject owner) {
			if (!owner)
				return;

			var controllers = owner.GetComponentsInChildren(typeof(SpriteShapeController));

			if (controllers.IsEmptyOrNull())
				return;

			foreach (var controller in controllers) {
				if (controller == null || !controller || !controller.gameObject)
					continue;

#if UNITY_EDITOR
				if (Application.isEditor && !Application.isPlaying) {
					Object.DestroyImmediate(controller.gameObject);
				}
				else
					Object.Destroy(controller.gameObject);
#else
				Object.Destroy(controller.gameObject);
#endif
			}
		}
	}
}