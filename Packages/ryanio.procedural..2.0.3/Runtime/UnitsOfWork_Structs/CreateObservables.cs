// ProceduralGeneration

using BCL;

namespace ProceduralGeneration {
	public readonly struct CreateObservables {
		public ObservableCollection<string> Create(ProceduralConfig config) {
			var observables = new ObservableCollection<string> {
				{
					StateObservableId.ON_CLEAN, new Observable(
						config.SerializedEvents[ProcessStep.Cleaning].Invoke)
				}, {
					StateObservableId.ON_INIT, new Observable(
						config.SerializedEvents[ProcessStep.Initializing].Invoke)
				}, {
					StateObservableId.ON_RUN, new Observable(
						config.SerializedEvents[ProcessStep.Running].Invoke)
				}, {
					StateObservableId.ON_COMPLETE, new Observable(
						config.SerializedEvents[ProcessStep.Completing].Invoke)
				}, {
					StateObservableId.ON_DISPOSE, new Observable(
						config.SerializedEvents[ProcessStep.Disposing].Invoke)
				}, {
					StateObservableId.ON_ERROR, new Observable(
						config.SerializedEvents[ProcessStep.Error].Invoke)
				}
			};

			return observables;
		}
	}
}