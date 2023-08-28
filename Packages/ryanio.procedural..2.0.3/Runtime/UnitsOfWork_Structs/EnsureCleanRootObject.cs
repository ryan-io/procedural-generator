// ProceduralGeneration

using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct EnsureCleanRootObject {
		public void Check(GameObject rootObject) {
			if (!rootObject)
				return;

			var components = rootObject.GetComponentsInChildren<Transform>();

			if (components.IsEmptyOrNull())
				return;

			foreach (var c in components) {
				if (!c || c == rootObject.transform)
					continue;

				var astarPaths = c.GetComponentsInChildren<AstarPath>();

				if (!astarPaths.IsEmptyOrNull())
					continue;
				
				Object.DestroyImmediate(c.gameObject);
			}
		}
	}
}