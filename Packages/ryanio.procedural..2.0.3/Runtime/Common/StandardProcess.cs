// ProceduralGeneration

using CommunityToolkit.HighPerformance;
using Pathfinding;

namespace ProceduralGeneration {
	/// <summary>
	///  The meat of the generation process. This is where the map is actually generated.
	///  This is an unsafe method.
	///  Treat this as more of a script/ini file than a class.
	///  This is customizable; if you prefer to implement your algorithms, you can do so by inheriting from the
	///		appropriate solver class and defining a new process (inherit from GenerationProcess).
	/// </summary>
	internal class StandardProcess : GenerationProcess {
		internal override void Run(Span2D<int> map) {
			var ctxCreator = new ContextCreator(Actions);

			FillMap(map, ctxCreator.GetNewFillMapCtx());
			SmoothMap(map, ctxCreator.GetNewSmoothMapCtx());
			RemoveRegions(map, ctxCreator.GetNewRemoveRegionsCtx());
			
			SetTiles(
				map,
				ctxCreator.GetNewTileSetterCtx(),
				ctxCreator.GetNewTileMapperCtx(),
				ctxCreator.GetNewTileToolsCtx());

			// what to do with the mesh solver data?
			CreateMesh(map, ctxCreator.GetNewMeshSolverCtx());
			
			BuildNavigation(new GridGraphBuilder(
				ctxCreator.GetNewGridGraphBuilderCtx()), 
				ctxCreator.GetNewNavigationSolverCtx());
		}

		static void FillMap(Span2D<int> map, FillMapSolverCtx ctx) {
			ProceduralService.GetFillMapSolver(() => new CellularAutomataFillMapSolver(ctx))
			                 .Fill(map);
		}

		static void SmoothMap(Span2D<int> map, SmoothMapSolverCtx ctx) {
			ProceduralService.GetSmoothMapSolver(() => new MarchingSquaresSmoothMapSolver(ctx))
			                 .Smooth(map);
		}

		static void RemoveRegions(Span2D<int> map, RemoveRegionsSolverCtx ctx) {
			ProceduralService.GetRegionRemovalSolver(() => new FloodRegionRemovalSolver(ctx))
			                 .Remove(map);
		}

		static void SetTiles(
			Span2D<int> map,
			TileSolversCtx tileSolverCtx,
			TileMapperCtx mapperCtx,
			GeneratorToolsCtx toolsCtx) {
			ProceduralService.GetTileSetterSolver(() => new StandardTileSetterSolver(
				                                      tileSolverCtx, mapperCtx, toolsCtx))
			                 .Set(map);
		}

		static void CreateMesh(Span2D<int> map, MeshSolverCtx ctx) {
			ProceduralService.GetMeshSolver(() => new MarchingSquaresMeshSolver(ctx))
			                 .Create(map);
		}

		static void BuildNavigation(NavGraphBuilder<GridGraph> builder, NavigationSolverCtx ctx) {
			ProceduralService.GetNavigationSolver(() => new NavigationSolver(builder, ctx))
			                 .Build();
		}

		internal StandardProcess(IActions actions) : base(actions) {
		}
	}
}