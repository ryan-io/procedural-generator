// ProceduralGeneration

using System;
using BCL;

namespace ProceduralGeneration {
	internal class GeneratorEvents {
		ObservableCollection<string> Observables { get; }

		internal void InvokeEvent(string eventIdentifier) {
			if (string.IsNullOrWhiteSpace(eventIdentifier))
				return;
		}
		
		internal void RegisterEvent(string eventIdentifier, Action action) {
			if (string.IsNullOrWhiteSpace(eventIdentifier))
				return;
			
			Observables[eventIdentifier].Register(action);
		}

		public GeneratorEvents(ProceduralConfig proceduralConfig) {
			Observables = Create.Observables(proceduralConfig);
		}
	}
}