using UnityEngine;

namespace Engine.Procedural.Poisson_Spawning {
	public abstract class PoissonMod : ScriptableObject {
		public abstract void Process(Transform tr);
	}
}