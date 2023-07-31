// Engine.Procedural

using System;

namespace Engine.Procedural.Runtime {
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