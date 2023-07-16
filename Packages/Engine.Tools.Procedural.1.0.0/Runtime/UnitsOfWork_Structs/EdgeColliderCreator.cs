using UnityEngine;

namespace Engine.Procedural {
	public readonly struct EdgeColliderCreator {
		public GameObject Create(ProceduralGenerator generator) {
			var trs = generator.gameObject.GetComponentsInChildren<Transform>();

			foreach (var tr in trs) {
				if (tr && tr.gameObject != generator.gameObject) {
#if UNITY_EDITOR
					Object.DestroyImmediate(tr.gameObject);
#else
					Object.Destroy(tr.gameObject);
#endif
				}
			}

			var o = new GameObject(Constants.EDGE_COLLIDER_GO_NAME) {
				transform = {
					parent = generator.transform
				},
				layer = LayerMask.NameToLayer("Obstacles")
			};

			o.transform.localPosition = o.transform.parent.position;
			return o;
		}
	}
}