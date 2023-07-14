// Engine.Procedural

using System;

namespace Engine.Procedural {
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