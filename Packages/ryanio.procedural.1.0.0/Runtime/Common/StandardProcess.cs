// ProceduralGeneration

using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using Pathfinding;
using Unity.Burst;
using Unity.Profiling;

namespace ProceduralGeneration {
	/// <summary>
	///  The meat of the generation process. This is where the map is actually generated.
	///  This is an unsafe method.
	///  Treat this as more of a script/ini file than a class.
	///  This is customizable; if you prefer to implement your algorithms, you can do so by inheriting from the
	///		appropriate solver class and defining a new process (inherit from GenerationProcess).
	/// </summary>
	internal class StandardProcess : GenerationProcess {
		internal override MapData Run(Span2D<int> map) {
			var ctxCreator        = new ContextCreator(Actions);
			var generatorToolsCtx = ctxCreator.GetNewGeneratorToolsCtx();

			FillMap(map, ctxCreator.GetNewFillMapCtx());
			SmoothMap(map, ctxCreator.GetNewSmoothMapCtx());
			Actions.SetRooms(ProcessRoomsAndWalls(map, ctxCreator.GetNewRemoveRegionsCtx()));

			SetTiles(map,
				ctxCreator.GetNewTileSetterCtx(),
				ctxCreator.GetNewTileMapperCtx(),
				generatorToolsCtx);

			Actions.SetMeshData(CreateMesh(map, ctxCreator.GetNewMeshSolverCtx()));

			SetGridCharacteristics(ctxCreator.GetNewGridCharacteristicsSolverCtx(), generatorToolsCtx);

			BuildNavigation(new GridGraphBuilder(
					ctxCreator.GetNewGridGraphBuilderCtx()),
				ctxCreator.GetNewNavigationSolverCtx());

			var coordinates = CreateColliders(ctxCreator.GetNewColliderSolverCtx());

			AssignCoordinates(coordinates);
			SetBoundaryColliderPoints(ctxCreator.GetNewColliderPointSetterCtx());
			GenerateSpriteShapeBorder(ctxCreator.GetNewSpriteShapeBorderCtx());

			return new MapData(Actions.GetTileHashset(), Actions.GetMeshData());
		}

		
		static void FillMap(Span2D<int> map, FillMapSolverCtx ctx) {
			using (FillMapMarker.Auto()) {
				ProceduralService.GetFillMapSolver(() => new CellularAutomataFillMapSolver(ctx))
				                 .Fill(map);
			}
		}

		
		static void SmoothMap(Span2D<int> map, SmoothMapSolverCtx ctx) {
			using (SmoothMapMarker.Auto()) {
				ProceduralService.GetSmoothMapSolver(() => new StandardSmoothMapSolver(ctx))	
				                 .Smooth(map, ctx.Dimensions);
			}
		}

		
		static List<Room> ProcessRoomsAndWalls(Span2D<int> map, RemoveRegionsSolverCtx ctx) {
			using (ProcessRoomsAndWallsMarker.Auto()) {
				return ProceduralService.GetRoomsAndWallsSolver(
					() => new FloodFillRegionSolver(ctx)).Remove(map);
			}
		}

		
		static void SetTiles(
			Span2D<int> map,
			TileSolversCtx tileSolverCtx,
			TileMapperCtx mapperCtx,
			GeneratorToolsCtx toolsCtx) {
			using (SetTilesMarker.Auto()) {
				ProceduralService.GetTileSetterSolver(
					() => new StandardTileSetterSolver(tileSolverCtx, mapperCtx, toolsCtx)).Set(map);
			}
		}

		
		static MeshData CreateMesh(Span2D<int> map, MeshSolverCtx ctx) {
			using (CreateMeshMarker.Auto()) {
				return ProceduralService.GetMeshSolver(() => new MarchingSquaresMeshSolver(ctx))
				                        .Create(map);
			}
		}

		
		static void BuildNavigation(NavGraphBuilder<GridGraph> builder, NavigationSolverCtx ctx) {
			using (BuildNavigationMarker.Auto()) {
				ProceduralService.GetNavigationSolver(() => new NavigationSolver(builder, ctx))
				                 .Build();
			}
		}

		
		static Coordinates CreateColliders(ColliderSolverCtx ctx) {
			using (CreateCollidersMarker.Auto()) {
				return ProceduralService.GetColliderSolver(() => new ColliderSolver(ctx)).Solve();
			}
		}

		
		static void GenerateSpriteShapeBorder(SpriteShapeBorderCtx ctx) {
			using (GenerateSpriteShapeBorderMarker.Auto()) {
				ProceduralService.GetSpriteShapeBorderSolver(() => new SpriteShapeBorderSolver(ctx)).Generate();
			}
		}

		
		static void SetBoundaryColliderPoints(ColliderPointSetterCtx ctx) {
			using (SetBoundaryColliderPointsBorderMarker.Auto()) {
				ProceduralService.GetCutCollidersSolver(() => new CreateBoundaryColliders(ctx)).Set();
			}
		}

		
		static void SetGridCharacteristics(GridCharacteristicsSolverCtx ctx, GeneratorToolsCtx toolsCtx) {
			using (SetGridCharacteristicsBorderMarker.Auto()) {
				ProceduralService.GetGridCharacteristicsSolver(
					() => new GridCharacteristicsSolver(ctx, toolsCtx)).Set();
			}
		}

		void AssignCoordinates(Coordinates coordinates) => Actions.SetCoords(coordinates);

		internal StandardProcess(IActions actions) : base(actions) {
		}

		static readonly ProfilerMarker FillMapMarker         = new(pConstant.PREFIX + nameof(FillMap));
		static readonly ProfilerMarker SmoothMapMarker       = new(pConstant.PREFIX + nameof(SmoothMap));
		static readonly ProfilerMarker SetTilesMarker        = new(pConstant.PREFIX + nameof(SetTiles));
		static readonly ProfilerMarker CreateMeshMarker      = new(pConstant.PREFIX + nameof(CreateMesh));
		static readonly ProfilerMarker CreateCollidersMarker = new(pConstant.PREFIX + nameof(CreateColliders));

		static readonly ProfilerMarker BuildNavigationMarker =
			new(pConstant.PREFIX + nameof(BuildNavigation) + " (A* Scan)");

		static readonly ProfilerMarker ProcessRoomsAndWallsMarker =
			new(pConstant.PREFIX + nameof(ProcessRoomsAndWalls));

		static readonly ProfilerMarker GenerateSpriteShapeBorderMarker =
			new(pConstant.PREFIX + nameof(GenerateSpriteShapeBorder));

		static readonly ProfilerMarker SetBoundaryColliderPointsBorderMarker =
			new(pConstant.PREFIX + nameof(SetBoundaryColliderPoints));

		static readonly ProfilerMarker SetGridCharacteristicsBorderMarker =
			new(pConstant.PREFIX + nameof(SetGridCharacteristics));
	}
}