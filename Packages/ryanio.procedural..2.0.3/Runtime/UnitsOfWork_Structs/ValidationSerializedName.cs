// Engine.Procedural

using System.Linq;
using UnityBCL.Serialization;

namespace Engine.Procedural.Runtime {
	public readonly struct ValidationSerializedName {
		public bool Validate(string nameSeedIteration, SerializerSetup seedSetup) {
			var seeds = GeneratorSerializer.GetAllSeeds(seedSetup).ToList();
			return seeds.Contains(nameSeedIteration);
		}
	}
}