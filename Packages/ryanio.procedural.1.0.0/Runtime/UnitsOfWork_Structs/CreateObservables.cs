// ProceduralGeneration

using BCL;
using StateMachine;
using UnityEngine;

namespace ProceduralGeneration {
	public static class Create {
		public static ObservableCollection<string> Observables(EventDictionary eventDictionary) {
			var observables = new ObservableCollection<string> {
				{
					StateObservableId.ON_CLEAN, new Observable(
						eventDictionary[ProcessStep.Cleaning].Invoke)
				}, {
					StateObservableId.ON_INIT, new Observable(
						eventDictionary[ProcessStep.Initializing].Invoke)
				}, {
					StateObservableId.ON_RUN, new Observable(
						eventDictionary[ProcessStep.Running].Invoke)
				}, {
					StateObservableId.ON_COMPLETE, new Observable(
						eventDictionary[ProcessStep.Completing].Invoke)
				}, {
					StateObservableId.ON_DISPOSE, new Observable(
						eventDictionary[ProcessStep.Disposing].Invoke)
				}, {
					StateObservableId.ON_ERROR, new Observable(
						eventDictionary[ProcessStep.Error].Invoke)
				}
			};

			return observables;
		}

		public static StateMachine<ProcessStep> StateMachine(GameObject owner) {
			return new StateMachine<ProcessStep>(owner, true);
		}
	}
}