// ProceduralGeneration

using System;
using BCL;
using JetBrains.Annotations;

namespace ProceduralGeneration {
	internal class GeneratorEvents {
		ObservableCollection<string> Observables { get; }

		internal void InvokeEvent(string eventIdentifier) {
			if (string.IsNullOrWhiteSpace(eventIdentifier))
				return;
		}
		
		internal void RegisterEvent(string eventIdentifier, [CanBeNull] Action action) {
			if (string.IsNullOrWhiteSpace(eventIdentifier))
				return;
			
			if (action != null)
				Observables[eventIdentifier].Register(action);
		}

		public GeneratorEvents(IActions actions) {
			Observables = Create.Observables(actions.GetProceduralConfig());
		}
	}
}