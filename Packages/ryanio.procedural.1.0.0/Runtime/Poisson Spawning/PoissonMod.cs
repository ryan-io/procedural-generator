using UnityEngine;

namespace ProceduralGeneration {
	public abstract class PoissonMod : ScriptableObject {
		public abstract void Process(Transform tr);
	}
}