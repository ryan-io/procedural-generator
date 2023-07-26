using UnityEngine;

namespace Engine.Procedural.Runtime {
	public readonly struct ColliderGameObjectCreator {
		public GameObject Create(ProceduralGenerator generator, string name) {
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

			var o = new GameObject(Constants.SAVE_COLLIDERS_PREFIX + name) {  
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