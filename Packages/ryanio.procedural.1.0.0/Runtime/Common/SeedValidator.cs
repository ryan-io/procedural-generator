// Engine.Procedural

using UnityBCL;

namespace ProceduralGeneration {
	public class SeedValidator {
		public void Validate(SeedInfo seedInfo) {
			if (_config.UseRandomSeed)
				_config.Seed = NumberSeeding.CreateRandomSeed();

			if (_config.LastSeed == _config.Seed)
				_config.LastIteration++;
			else
				_config.LastIteration = 0;

			_config.LastSeed = _config.Seed;

			_config.NameSeedIteration =
				_config.Name         +
				Constants.UNDERSCORE +
				seedInfo.Seed +
				Constants.UID        +
				seedInfo.Iteration;
		}

		public SeedValidator(ProceduralConfig config) {
			_config = config;
		}

		readonly ProceduralConfig _config;
	}
}