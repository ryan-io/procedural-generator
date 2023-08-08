// Engine.Procedural

using System.Linq;

namespace Engine.Procedural.Runtime {
	public class ValidationSerializedName {
		public bool Validate(string nameSeedIteration) {
			var seeds = Help.GetAllSeeds().ToList();
			return seeds.Contains(nameSeedIteration);
		}
	}
}