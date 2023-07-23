// Engine.Procedural

using UnityBCL;

namespace Engine.Procedural.Runtime {
	public class SeedValidator {
		public void Validate() {
			if (_config.UseRandomSeed)
				_config.Seed = NumberSeeding.CreateRandomSeed();

			if (_config.LastSeed == _config.Seed)
				_config.LastIteration++;
			else
				_config.LastIteration = 0;

			_config.LastSeed = _config.Seed;
		}

		public SeedValidator(ProceduralConfig config) {
			_config = config;
		}

		readonly ProceduralConfig _config;
	}
}