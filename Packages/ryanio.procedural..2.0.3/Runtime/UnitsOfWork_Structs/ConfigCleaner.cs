// Engine.Procedural

namespace ProceduralGeneration {
	public readonly struct ConfigCleaner {
		public void Clean(ProceduralConfig config) {
			config.Name = SanitizeName(config.Name);
		}

		string SanitizeName(string configName) {
			if (string.IsNullOrWhiteSpace(configName))
				configName = "proceduralMap";

			return configName;
		}
	}
}