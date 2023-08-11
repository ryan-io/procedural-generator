// ProceduralGeneration

using StateMachine;

namespace ProceduralGeneration {
	internal class GeneratorState {
		StateMachine<ProcessStep> StateMachine { get; }

		internal void ChangeState(ProcessStep state) {
			StateMachine.ChangeState(state);
		}

		internal GeneratorState(IOwner owner, GeneratorEvents events) {
			StateMachine = Create.StateMachine(owner);
		}
	}
}