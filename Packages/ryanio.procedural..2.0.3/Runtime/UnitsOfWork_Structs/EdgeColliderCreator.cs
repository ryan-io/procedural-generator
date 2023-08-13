using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct ColliderGameObjectCreator {
		public GameObject Create(GameObject owner) {
			var trs = owner.GetComponentsInChildren<Transform>();

			foreach (var tr in trs) {
				if (tr && tr.gameObject != owner.gameObject) {
#if UNITY_EDITOR
					Object.DestroyImmediate(tr.gameObject);
#else
					Object.Destroy(tr.gameObject);
#endif
				}
			}

			var o = new GameObject {  
				transform = {
					parent = owner.transform
				},
				layer = LayerMask.NameToLayer(Constants.Layer.OBSTACLES)
			};

			o.transform.localPosition = o.transform.parent.position;
			return o;
		}
	}
}