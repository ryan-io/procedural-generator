// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct SetColliderObjName {
		public void Set(GameObject colliderObj, string name) {
			colliderObj.name = name;
		}
	}
}