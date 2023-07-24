// Engine.Procedural

using System.Linq;
using Standalone.Serialization;

namespace Engine.Procedural.Runtime {
	public readonly struct ValidationSerializedName {
		public bool Validate(string nameSeedIteration, SerializerSetup seedSetup) {
			var seeds = ProceduralSerializer.GetAllSeeds(seedSetup).ToList();
			return seeds.Contains(nameSeedIteration);
		}
	}
}