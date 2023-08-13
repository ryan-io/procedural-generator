// ProceduralGeneration

using System;

namespace ProceduralGeneration {
	internal class InitializationService {
		IActions Actions { get; }

		internal void Run(IMachine machine) {
			machine.InvokeEvent(StateObservableId.ON_INIT);
			new GeneratorCleaner(Actions).Clean(machine);
			new ColliderGameObject(Actions).Setup();
			new ActiveAstarData().Retrieve();
		}

		public InitializationService(IActions actions) {
			Actions = actions;
		}
	}
}