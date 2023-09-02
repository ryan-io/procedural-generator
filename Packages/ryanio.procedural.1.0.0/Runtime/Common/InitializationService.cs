// ProceduralGeneration

namespace ProceduralGeneration {
	internal class InitializationService {
		IActions Actions { get; }

		internal void Run(ProceduralConfig config) {
			new GeneratorCleaner(Actions).Clean(config, true);
			new ColliderGameObject(Actions).Setup();
			ActiveAstarData.Retrieve();
		}

		public InitializationService(IActions actions) {
			Actions = actions;
		}
	}
}