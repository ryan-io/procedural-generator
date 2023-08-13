// ProceduralGeneration

namespace ProceduralGeneration {
	internal class GeneratorSceneSetupService {
		IActions Actions { get; }

		internal void Run() {
			var containerBuilder = new ContainerBuilder(Actions.GetOwner());
			var output           = containerBuilder.Build();

			Actions.SetTileMapDictionary(output.dictionary);
			Actions.SetGrid(output.grid);

			new GridPosition().Set(Actions.GetProceduralConfig(), Actions.GetGrid());
		}
		
		public GeneratorSceneSetupService(IActions actions) {
			Actions = actions;
		}
	}
}