﻿// Engine.Procedural

using System.Runtime.CompilerServices;

namespace Engine.Procedural {
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