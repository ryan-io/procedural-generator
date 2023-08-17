// ProceduralGeneration

using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.HighPerformance;
using Pathfinding;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct Serialize {
		internal Dictionary<int, List<SerializableVector3>> SerializeVector3Collection(Dictionary<int, List<Vector3>> col) {
			var output = new Dictionary<int, List<SerializableVector3>>();
			var index = col.Keys.First();

			foreach (var corner in col) {
				var serializedList = corner.Value.AsSerialized().ToList();
				output.Add(index, serializedList);
				index++;
			}
			
			// for (var i = index; i < col.Count; i++) {
			// 	output[i] = col[i].AsSerialized().ToList();
			// }

			return output;
		}
		
		/*	BEFORE REFACTOR
		 * internal Dictionary<int, List<SerializableVector3>> GetBoundaryCoords() {
			if (SpriteBoundaryCoords.IsEmptyOrNull())
				return default;

			var dict  = new Dictionary<int, List<SerializableVector3>>();
			var index = SpriteBoundaryCoords.Keys.First();

			foreach (var corner in SpriteBoundaryCoords) {
				var serializedList = corner.Value.AsSerialized().ToList();
				dict.Add(index, serializedList);
				index++;
			}

			return dict;
		}
		 */
	}
	/// <summary>
	///  The meat of the generation process. This is where the map is actually generated.
	///  This is an unsafe method.
	///  Treat this as more of a script/ini file than a class.
	///  This is customizable; if you prefer to implement your algorithms, you can do so by inheriting from the
	///		appropriate solver class and defining a new process (inherit from GenerationProcess).
	/// </summary>
	internal class StandardProcess : GenerationProcess {
		internal override MapData Run(Span2D<int> map) {
			var ctxCreator = new ContextCreator(Actions);
			var data       = new MapData(Actions.GetTileHashset(), Actions.GetMeshData());

			FillMap(map, ctxCreator.GetNewFillMapCtx());
			SmoothMap(map, ctxCreator.GetNewSmoothMapCtx());
			RemoveRegions(map, ctxCreator.GetNewRemoveRegionsCtx());

			SetTiles(map,
				ctxCreator.GetNewTileSetterCtx(),
				ctxCreator.GetNewTileMapperCtx(),
				ctxCreator.GetNewTileToolsCtx());

			var meshData = CreateMesh(map, ctxCreator.GetNewMeshSolverCtx());
			Actions.SetMeshData(meshData);

			BuildNavigation(new GridGraphBuilder(
					ctxCreator.GetNewGridGraphBuilderCtx()),
				ctxCreator.GetNewNavigationSolverCtx());

			var coordinates = CreateColliders(ctxCreator.GetNewColliderSolverCtx());
			AssignCoordinates(data, coordinates);

			return new MapData();
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

		static MeshData CreateMesh(Span2D<int> map, MeshSolverCtx ctx) {
			return ProceduralService.GetMeshSolver(() => new MarchingSquaresMeshSolver(ctx))
			                        .Create(map);
		}

		static void BuildNavigation(NavGraphBuilder<GridGraph> builder, NavigationSolverCtx ctx) {
			ProceduralService.GetNavigationSolver(() => new NavigationSolver(builder, ctx))
			                 .Build();
		}

		static Coordinates CreateColliders(ColliderSolverCtx ctx) {
			return ProceduralService.GetColliderSolver(() => new ColliderSolver(ctx)).Solve();
		}

		static void AssignCoordinates(MapData data, Coordinates coordinates) {
			data.SpriteBoundaryCoords = coordinates.SpriteBoundaryCoords;
			data.ColliderCoords       = coordinates.ColliderCoords;
		}

		internal StandardProcess(IActions actions) : base(actions) {
		}
	}
}