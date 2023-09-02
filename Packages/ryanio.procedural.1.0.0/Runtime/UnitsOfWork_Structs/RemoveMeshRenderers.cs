// Engine.Procedural

using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct RenderCleaner {
		public void Clean(GameObject owner) {
			var objs = owner.GetComponentsInChildren<MeshRenderer>();

			if (objs.IsEmptyOrNull())
				return;

			foreach (var o in objs) {
#if UNITY_EDITOR
				if (Application.isEditor)
					Object.DestroyImmediate(o.gameObject);
#else
					Object.Destroy(o.gameObject);
#endif
			}
		}
	}
}