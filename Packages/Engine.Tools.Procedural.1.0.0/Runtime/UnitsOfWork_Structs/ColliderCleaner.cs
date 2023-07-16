// Engine.Procedural

using System;
using UnityBCL;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Engine.Procedural {
	public readonly struct ColliderGameObjectCleaner {
		public void Clean(GameObject procGenGo) {
			var edgeColliders = procGenGo.GetComponentsInChildren(typeof(EdgeCollider2D));
			var boxColliders  = procGenGo.GetComponentsInChildren(typeof(BoxCollider));

			RemoveEdgeColliders(edgeColliders);
			RemoveBoxColliders(boxColliders);
		}

		static void RemoveEdgeColliders(Component[] edgeColliders) {
			if (!edgeColliders.IsEmptyOrNull()) {
				var edgeColliderSpan = new Span<Component>(edgeColliders);

				foreach (var edgeCollider in edgeColliderSpan) {
					if (edgeCollider && edgeCollider.gameObject) {
#if UNITY_EDITOR
						Object.DestroyImmediate(edgeCollider.gameObject);

#else
						Object.Destroy(edgeCollider.gameObject);
#endif
					}
				}
			}
		}

		static void RemoveBoxColliders(Component[] boxColliders) {
			if (!boxColliders.IsEmptyOrNull()) {
				var boxColliderSpan = new Span<Component>(boxColliders);

				foreach (var boxCollider in boxColliderSpan) {
					if (boxCollider && boxCollider.gameObject) {
#if UNITY_EDITOR
						Object.DestroyImmediate(boxCollider.gameObject);

#else
						Object.Destroy(boxCollider.gameObject);
#endif
					}
				}
			}
		}
	}
}