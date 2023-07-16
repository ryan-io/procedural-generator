using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public readonly struct EdgeColliderCreator {
		public GameObject Create(ProceduralGenerator generator) {
			generator.gameObject.RemoveChildGameObjects();
			
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