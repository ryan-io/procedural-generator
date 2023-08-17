// ProceduralGeneration

namespace ProceduralGeneration {
	internal class ContextCreator {
		IActions Actions { get; }

		internal FillMapSolverCtx GetNewFillMapCtx()
			=> new(Actions.GetWallFillPercentage(), Actions.GetSeed().Seed.GetHashCode());

		internal SmoothMapSolverCtx GetNewSmoothMapCtx() => new(
			Actions.GetMapDimensions(),
			Actions.GetUpperNeighborLimit(),
			Actions.GetLowerNeighborLimit(),
			Actions.GetSmoothingIterations());

		internal RemoveRegionsSolverCtx GetNewRemoveRegionsCtx() => new(
			Actions.GetMapDimensions(),
			Actions.GetCorridorWidth(),
			Actions.GetWallRemoveThreshold(),
			Actions.GetRoomRemoveThreshold());

		internal TileSolversCtx GetNewTileSetterCtx() => new(
			Actions.GetMapDimensions(),
			Actions.GetTilemapDictionary(),
			Actions.GetTileDictionary(),
			Actions.GetTileHashset(),
			Actions.GetGrid());

		internal GeneratorToolsCtx GetNewTileToolsCtx() => new(Actions.GetMapDimensions(), Actions.GetGrid());

		internal TileMapperCtx GetNewTileMapperCtx() => new(Actions.GetShouldCreateTileLabels());

		internal MeshSolverCtx GetNewMeshSolverCtx() => new(Actions.GetOwner(), Actions.GetSerializationName());
		
		internal NavigationSolverCtx GetNewNavigationSolverCtx() => new(Actions.GetTilemapDictionary());
		
		internal GridGraphBuilderCtx GetNewGridGraphBuilderCtx() => new(
			Actions.GetMapDimensions(),
			Actions.GetColliderType(), 
			Actions.GetObstacleMask(), 
			Actions.GetGraphCollideDiameter(), 
			Actions.GetNodeSize());

		internal ContextCreator(IActions actions) {
			Actions = actions;
		}

		public ColliderSolverCtx GetNewColliderSolverCtx() {
			return new(
				Actions.GetOwner(),
				Actions.GetColliderSolverType(),
				Actions.GetTilemapDictionary(),
				Actions.GetColliderGameObject(),
				Actions.GetMeshData().MeshVertices,
				Actions.GetMeshData().RoomOutlines,
				Actions.GetObstacleMask(),
				Actions.GetSkinWidth(),
				Actions.GetEdgeColliderOffset(),
				Actions.GetEdgeColliderRadius(),
				Actions.GetMapDimensions(),
				Actions.GetBorderSize());
		}
	}
}