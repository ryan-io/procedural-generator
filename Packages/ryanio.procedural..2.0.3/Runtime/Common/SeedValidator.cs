// Engine.Procedural

using UnityBCL;

namespace Engine.Procedural.Runtime {
	public class SeedValidator {
		public void Validate(ISeedInfo info) {
			if (_config.UseRandomSeed)
				_config.Seed = NumberSeeding.CreateRandomSeed();

			if (_config.LastSeed == _config.Seed)
				_config.LastIteration++;
			else
				_config.LastIteration = 0;

			_config.LastSeed = _config.Seed;
			var seedInfo = info.GetSeedInfo();

			_config.NameSeedIteration =
				_config.Name         +
				Constants.UNDERSCORE +
				seedInfo.CurrentSeed +
				Constants.UID        +
				seedInfo.LastIteration;
		}

		public SeedValidator(ProceduralConfig config) {
			_config = config;
		}

		readonly ProceduralConfig _config;
	}
}