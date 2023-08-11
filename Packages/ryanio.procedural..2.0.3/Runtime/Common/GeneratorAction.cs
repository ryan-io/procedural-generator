// ProceduralGeneration

namespace ProceduralGeneration {
	internal static class GeneratorAction {
		internal static void RegisterStateToEvents(GeneratorEvents events, GeneratorState state) {
			events.RegisterEvent(StateObservableId.ON_CLEAN,    () => state.ChangeState(ProcessStep.Cleaning));
			events.RegisterEvent(StateObservableId.ON_INIT,     () => state.ChangeState(ProcessStep.Initializing));
			events.RegisterEvent(StateObservableId.ON_RUN,      () => state.ChangeState(ProcessStep.Running));
			events.RegisterEvent(StateObservableId.ON_COMPLETE, () => state.ChangeState(ProcessStep.Completing));
			events.RegisterEvent(StateObservableId.ON_DISPOSE,  () => state.ChangeState(ProcessStep.Disposing));
			events.RegisterEvent(StateObservableId.ON_ERROR,    () => state.ChangeState(ProcessStep.Error));
		}
	}
}