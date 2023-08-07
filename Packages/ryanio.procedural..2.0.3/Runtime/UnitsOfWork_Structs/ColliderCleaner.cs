// Engine.Procedural

using System;
using ProceduralAuxiliary;
using UnityBCL;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Engine.Procedural.Runtime {
	public readonly struct ColliderGameObjectCleaner {
		public void Clean(GameObject procGenGo, bool destroyParent) {
			var edgeColliders      = procGenGo.GetComponentsInChildren(typeof(EdgeCollider2D));
			var boxColliders       = procGenGo.GetComponentsInChildren(typeof(BoxCollider));
			var primitiveColliders = procGenGo.GetComponentsInChildren(typeof(ProceduralPrimitiveCollider));

			if (destroyParent) {
				if (!edgeColliders.IsEmptyOrNull() && edgeColliders[0].transform.parent) {
					var o = edgeColliders[0].transform.parent.gameObject;
					if (o) {
#if UNITY_EDITOR
						Object.DestroyImmediate(o);
#else
						Object.Destroy(o);
#endif
						return;
					}
				}

				else if (!boxColliders.IsEmptyOrNull() && edgeColliders[0].transform.parent) {
					var o = boxColliders[0].transform.parent.gameObject;
					if (o) {
#if UNITY_EDITOR
						Object.DestroyImmediate(o);
#else
						Object.Destroy(o);
#endif
						return;
					}
				}

				else if (!primitiveColliders.IsEmptyOrNull() && edgeColliders[0].transform.parent) {
					var o = primitiveColliders[0].transform.parent.gameObject;
					if (o) {
#if UNITY_EDITOR
						Object.DestroyImmediate(o);
#else
						Object.Destroy(o);
#endif
						return;
					}
				}

			}

			RemovePrimitiveColliders(primitiveColliders);
			RemoveEdgeColliders(edgeColliders);
			RemoveBoxColliders(boxColliders);
		}


		static void RemovePrimitiveColliders(Component[] primitiveColliders) {
			if (!primitiveColliders.IsEmptyOrNull()) {
				var span = new Span<Component>(primitiveColliders);

				foreach (var p in span) {
					if (p && p.gameObject) {
#if UNITY_EDITOR
						Object.DestroyImmediate(p.gameObject);

#else
						Object.Destroy(p.gameObject);
#endif
					}
				}
			}
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