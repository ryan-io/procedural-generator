// ProceduralGeneration

namespace ProceduralGeneration {
	internal class GeneratorSceneSetupService {
		IActions Actions { get; }

		internal void Run() {
			var containerBuilder = new ContainerBuilder(Actions.GetOwner());
			var output           = containerBuilder.Build();

			Actions.SetTileMapDictionary(output.dictionary);
			Actions.SetGrid(output.grid);

			//GridPosition.Set(Actions.GetMapDimensions(), Actions.GetGrid(), Actions.GetBorderSize());
		}
		
		public GeneratorSceneSetupService(IActions actions) {
			Actions = actions;
		}
	}
}