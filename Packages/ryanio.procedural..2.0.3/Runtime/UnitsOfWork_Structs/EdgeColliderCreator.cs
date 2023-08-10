using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct ColliderGameObjectCreator {
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

			var o = new GameObject {  
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