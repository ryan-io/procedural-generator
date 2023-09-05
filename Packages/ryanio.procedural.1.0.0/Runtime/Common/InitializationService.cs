// ProceduralGeneration

namespace ProceduralGeneration {
	internal class InitializationService {
		IActions Actions { get; }

		internal void Run(ProceduralConfig config) {
			Constants.Instance.SetCellSize(config.CellSize);
			new GeneratorCleaner(Actions).Clean(config, true);
			new ColliderGameObject(Actions).Setup();
			ActiveAstarData.Retrieve();
		}

		public InitializationService(IActions actions) {
			Actions = actions;
		}
	}
}