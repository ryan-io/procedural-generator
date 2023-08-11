// ProceduralGeneration

using BCL;

namespace ProceduralGeneration {
	internal class GeneratorEvents {
		ObservableCollection<string> Observables { get; }

		internal void InvokeEvent(string eventIdentifier) {
			if (string.IsNullOrWhiteSpace(eventIdentifier))
				return;
		}

		public GeneratorEvents(ProceduralConfig proceduralConfig) {
			Observables = Create.Observables(proceduralConfig);
		}
	}
}