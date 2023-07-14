using System;

namespace Engine.Procedural {
	[Serializable]
	public enum CreationState {
		Pending,
		Cleaning,
		Initializing,
		Enabling,
		Starting,
		InProgress,
		Cancelling,
		Ending,
		Complete,
		Disposing,
		DidNotGenerate
	}
}