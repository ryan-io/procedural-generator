// ProceduralGeneration

using System;
using System.Collections.Generic;
using Pathfinding;
using Unity.Profiling;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///  The meat of the generation process. This is where the map is actually generated.
	///  This is an unsafe method.
	///  Treat this as more of a script/ini file than a class.
	///  This is customizable; if you prefer to implement your algorithms, you can do so by inheriting from the
	///		appropriate solver class and defining a new process (inherit from GenerationProcess).
	/// </summary>
	internal class StandardProcess : GenerationProcess, IDisposable {
		bool IsDisposed { get; set; }

		internal override MapData Run(ref int[,] map) {
			var ctxCreator        = new ContextCreator(Actions);
			var generatorToolsCtx = ctxCreator.GetNewGeneratorToolsCtx();

			FillMap(ref map, ctxCreator.GetNewFillMapCtx());
			SmoothMap(ref map, ctxCreator.GetNewSmoothMapCtx());

			Actions.SetRooms(ProcessRoomsAndWalls(map, ctxCreator.GetNewRemoveRegionsCtx()));
			Actions.SetMeshData(CreateMesh(map, ctxCreator.GetNewMeshSolverCtx()));

			SetGridCharacteristics(ctxCreator.GetNewGridCharacteristicsSolverCtx(), generatorToolsCtx);

			BuildNavigation(ref map,
				new GridGraphBuilder(ctxCreator.GetNewGridGraphBuilderCtx()),
				ctxCreator.GetNewNavigationSolverCtx());

			var coordinates = CreateColliders(ctxCreator.GetNewColliderSolverCtx());

			AssignCoordinates(coordinates);
			SetBoundaryColliderPoints(ctxCreator.GetNewColliderPointSetterCtx());
			GenerateSpriteShapeBorder(ctxCreator.GetNewSpriteShapeBorderCtx());

			Help.FlipYAxisSkipAstar(Actions.GetOwner(), 1);
			
			return new MapData(map, Actions.GetTileHashset(), Actions.GetMeshData());
		}


		static void FillMap(ref int[,] map, FillMapSolverCtx ctx) {
			using (FillMapMarker.Auto()) {
				ProceduralService.GetFillMapSolver(() => new CellularAutomataFillMapSolver(ctx))
				                 .Fill(ref map);
			}
		}


		static void SmoothMap(ref int[,] map, SmoothMapSolverCtx ctx) {
			using (SmoothMapMarker.Auto()) {
				ProceduralService.GetSmoothMapSolver(() => new StandardSmoothMapSolver(ctx))
				                 .Smooth(ref map, ctx.Dimensions);
			}
		}

		static List<Room> ProcessRoomsAndWalls(int[,] map, RemoveRegionsSolverCtx ctx) {
			using (ProcessRoomsAndWallsMarker.Auto()) {
				return ProceduralService.GetRoomsAndWallsSolver(
					() => new FloodFillRegionSolver(ctx)).Remove(ref map);
			}
		}


		static void SetTiles(
			ref int[,] map,
			TileSolversCtx tileSolverCtx,
			TileMapperCtx mapperCtx,
			GeneratorToolsCtx toolsCtx) {
			using (SetTilesMarker.Auto()) {
				ProceduralService.GetTileSetterSolver(
					() => new StandardTileSetterSolver(tileSolverCtx, mapperCtx, toolsCtx)).Set(ref map);
				//new StandardTileSetterSolver(tileSolverCtx,mapperCtx, toolsCtx)).Set(ref map);
			}
		}


		static MeshData CreateMesh(int[,] map, MeshSolverCtx ctx) {
			using (CreateMeshMarker.Auto()) {
				return ProceduralService.GetMeshSolver(() => new MarchingSquaresMeshSolver(ctx))
				                        .Create(ref map);
			}
		}


		static void BuildNavigation(ref int[,] map, NavGraphBuilder<GridGraph> builder, NavigationSolverCtx ctx) {
			using (BuildNavigationMarker.Auto()) {
				ProceduralService.GetNavigationSolver(() => new NavigationSolver(builder, ctx))
				                 .Build(ref map);
			}
		}


		Coordinates CreateColliders(ColliderSolverCtx ctx) {
			using (CreateCollidersMarker.Auto()) {
				var colliderCreator = ProceduralService.GetColliderSolver(() => new ColliderSolver(ctx));

				_disposables.Add(colliderCreator);

				return colliderCreator.Solve();
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

		public void Dispose() {
			if (_disposables.IsEmptyOrNull() || IsDisposed)
				return;

			IsDisposed = true;

			foreach (var disposable in _disposables) {
				disposable?.Dispose();
			}
		}

		readonly HashSet<IDisposable> _disposables = new();
	}
}