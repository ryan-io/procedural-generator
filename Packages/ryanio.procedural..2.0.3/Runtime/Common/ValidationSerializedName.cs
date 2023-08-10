// Engine.Procedural

using System.Linq;

namespace ProceduralGeneration {
	public class ValidationSerializedName {
		public bool Validate(string nameSeedIteration) {
			var seeds = Help.GetAllSeeds().ToList();
			return seeds.Contains(nameSeedIteration);
		}
	}
}