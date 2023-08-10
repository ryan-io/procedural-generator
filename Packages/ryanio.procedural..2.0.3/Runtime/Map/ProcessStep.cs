// Engine.Procedural

using System;

namespace ProceduralGeneration {
	[Serializable]
	public enum ProcessStep {
		Cleaning,
		Initializing,
		Running,
		Completing,
		Disposing,
		Error
	}
}