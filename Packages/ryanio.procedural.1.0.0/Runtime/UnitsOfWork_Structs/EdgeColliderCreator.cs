using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct ColliderGameObjectCreator {
		public GameObject Create(GameObject owner) {
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