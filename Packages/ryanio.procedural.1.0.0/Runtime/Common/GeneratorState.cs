// ProceduralGeneration

using StateMachine;

namespace ProceduralGeneration {
	internal class GeneratorState {
		StateMachine<ProcessStep> Machine { get; }

		internal void ChangeState(ProcessStep state) {
			Machine.ChangeState(state);
		}

		internal GeneratorState(IActions actions) {
			Machine = Create.StateMachine(actions.GetOwner());
		}
	}
}