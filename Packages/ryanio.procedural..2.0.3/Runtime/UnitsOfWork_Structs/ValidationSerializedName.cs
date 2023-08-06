// Engine.Procedural

using System.Linq;
using UnityBCL.Serialization;

namespace Engine.Procedural.Runtime {
	public class ValidationSerializedName {
		public bool Validate(string nameSeedIteration) {
			var seeds = GeneratorSerializer.GetAllSeeds().ToList();
			return seeds.Contains(nameSeedIteration);
		}
	}
}