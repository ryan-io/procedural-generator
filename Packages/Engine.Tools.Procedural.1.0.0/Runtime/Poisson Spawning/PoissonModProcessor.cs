using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural.Poisson_Spawning {
	public static class PoissonModProcessor {
		public static void Process(Transform tr, IEnumerable<PoissonMod> mods) {
			if (tr == null) return;
			foreach (var mod in mods) {
				if (mod == null) continue;
				mod.Process(tr);
			}
		}
	}
}