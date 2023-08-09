// ProceduralGeneration

using UnityEngine;

namespace Engine.Procedural.Runtime {
	public readonly struct SetColliderObjName {
		public void Set(GameObject colliderObj, string name) {
			colliderObj.name = name;
		}
	}
}