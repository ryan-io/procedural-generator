// ProceduralGeneration

namespace ProceduralGeneration {
	internal static class GeneratorAction {
		internal static void
			RegisterStateChangeToEvents(IActions actions, GeneratorEvents events, GeneratorState state) {
			events.RegisterEvent(StateObservableId.ON_CLEAN,    () => state.ChangeState(ProcessStep.Cleaning));
			events.RegisterEvent(StateObservableId.ON_INIT,     () => state.ChangeState(ProcessStep.Initializing));
			events.RegisterEvent(StateObservableId.ON_RUN,      () => state.ChangeState(ProcessStep.Running));
			events.RegisterEvent(StateObservableId.ON_COMPLETE, () => state.ChangeState(ProcessStep.Completing));
			events.RegisterEvent(StateObservableId.ON_DISPOSE,  () => state.ChangeState(ProcessStep.Disposing));
			events.RegisterEvent(StateObservableId.ON_ERROR,    () => state.ChangeState(ProcessStep.Error));

			events.RegisterEvent(StateObservableId.ON_CLEAN,
				() => actions.Log(Message.STATE_TO_CLEAN, nameof(StateMachine)));

			events.RegisterEvent(StateObservableId.ON_INIT,
				() => actions.Log(Message.STATE_TO_INIT, nameof(StateMachine)));

			events.RegisterEvent(StateObservableId.ON_RUN,
				() => actions.Log(Message.STATE_TO_RUN, nameof(StateMachine)));

			events.RegisterEvent(StateObservableId.ON_COMPLETE,
				() => actions.Log(Message.STATE_TO_COMPLETE, nameof(StateMachine)));

			events.RegisterEvent(StateObservableId.ON_DISPOSE,
				() => actions.Log(Message.STATE_TO_DISPOSE, nameof(StateMachine)));

			events.RegisterEvent(StateObservableId.ON_ERROR,
				() => actions.Log(Message.STATE_TO_ERROR, nameof(StateMachine)));
		}
	}
}