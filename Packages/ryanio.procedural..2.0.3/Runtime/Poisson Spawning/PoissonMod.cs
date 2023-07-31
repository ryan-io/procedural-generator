using UnityEngine;

namespace Engine.Procedural.Runtime {
	public abstract class PoissonMod : ScriptableObject {
		public abstract void Process(Transform tr);
	}
}