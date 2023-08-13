// ProceduralGeneration

namespace ProceduralGeneration {
	internal class GeneratorSetupService {
		internal void Run() {
			Tools           = new GeneratorTools(_config, Grid, StopWatch);
			FillMapSolver   = new CellularAutomataFillMapSolver(_config, StopWatch);
			SmoothMapSolver = new MarchingSquaresSmoothMapSolver(_config, StopWatch);
			TileTypeSolver =
				new SetAllTilesSyncTileTypeSolver(_config, TileHashset, TileMapDictionary, Grid, StopWatch);
			NodeSerializationSolver = new NodeSerializationSolver(_config, this, TileMapDictionary, StopWatch);
			RegionRemoverSolver     = new FloodRegionRemovalSolver(_config);
			ErosionSolver           = new ErosionSolver(_config, TileHashset, TileMapDictionary);
			MeshSolver              = new MarchingSquaresMeshSolver(this);
			ColliderSolver          = new ColliderSolver(_config, gameObject, GeneratedCollidersObj, StopWatch);
			GridGraphBuilder        = new GridGraphBuilder(_config);
			NavGraphRulesSolver     = new NavGraphRulesSolver(TileMapDictionary);
			GraphScanner            = new GraphScanner(StopWatch);
			GeneratorSerializer     = new GeneratorSerializer(_config, Grid.gameObject, StopWatch);
			Rendering               = new MeshRendering(gameObject, default);
		}
	}
}