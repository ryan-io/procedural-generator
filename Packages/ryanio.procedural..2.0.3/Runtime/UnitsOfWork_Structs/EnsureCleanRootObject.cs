// ProceduralGeneration

using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public readonly struct EnsureCleanRootObject {
		public void Check(GameObject rootObject) {
			if (!rootObject)
				return;

			var components = rootObject.GetComponentsInChildren<Transform>();

			if (components.IsEmptyOrNull())
				return;

			foreach (var c in components) {
				if (c == null || !c || c == rootObject.transform)
					continue;

				Object.DestroyImmediate(c.gameObject);
			}
		}
	}
}