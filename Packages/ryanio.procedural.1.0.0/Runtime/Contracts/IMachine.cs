// ProceduralGeneration

using System;
using JetBrains.Annotations;

namespace ProceduralGeneration {
	public interface IMachine {
		void RegisterEvent(string identifier, [CanBeNull] Action action);
		void InvokeEvent(string identifier);
	}
}