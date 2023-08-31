// ProceduralGeneration

using System;
using System.Collections.Generic;
using UnityEngine;

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

		internal GeneratorToolsCtx GetNewGeneratorToolsCtx() => new(Actions.GetMapDimensions(), Actions.GetGrid());

		internal TileMapperCtx GetNewTileMapperCtx() => new(Actions.GetShouldCreateTileLabels());

		internal MeshSolverCtx GetNewMeshSolverCtx() => new(Actions.GetOwner(), Actions.GetSerializationName());

		internal NavigationSolverCtx GetNewNavigationSolverCtx() => new(
			Actions.GetTilemapDictionary(),
			Actions.GetGraphColliderCutters(),
			Actions.GetTileHashset());

		internal GridGraphBuilderCtx GetNewGridGraphBuilderCtx() => new(
			Actions.GetMapDimensions(),
			Actions.GetColliderType(),
			Actions.GetObstacleMask(),
			Actions.GetGraphCollideDiameter(),
			Actions.GetNodeSize());

		internal ContextCreator(IActions actions) {
			Actions = actions;
		}

		internal ColliderSolverCtx GetNewColliderSolverCtx() {
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

		internal SpriteShapeBorderCtx GetNewSpriteShapeBorderCtx() {
			return new(
				Actions.GetSpriteShapeConfig(),
				Actions.GetOwner(),
				Actions.GetCoordinates().ProcessedCoords,
				Actions.GetSerializationName());
		}

		internal SpriteShapeBorderCtx GetNewSpriteShapeBorderCtx(IReadOnlyDictionary<int, List<Vector3>> coords) {
			return new(
				Actions.GetSpriteShapeConfig(),
				Actions.GetOwner(),
				coords,
				Actions.GetSerializationName());
		}

		internal GridCharacteristicsSolverCtx GetNewGridCharacteristicsSolverCtx() {
			return new(
				Actions.GetGrid(),
				Actions.GetSerializationName());
		}

		internal ColliderPointSetterCtx GetNewColliderPointSetterCtx() {
			return new(
				Actions.GetColliderGameObject(),
				Actions.GetTileHashset(),
				Actions.GetMapDimensions(),
				Actions.GetBorderSize(),
				Actions.GetEdgeColliderRadius());
		}

		internal SerializationRouterCtx GetNewSerializationRouterCtx() {
			return new SerializationRouterCtx(Actions.GetSeed(), Actions.GetGrid(), Actions.GetSerializationName());
		}

		internal SerializationRoute GetNewSerializationRoute() {
			return new SerializationRoute(
				Actions.GetShouldSerializePathfinding(),
				Actions.GetShouldSerializeMapPrefab(),
				Actions.GetShouldSerializeSpriteShape(),
				Actions.GetShouldSerializeColliderCoords(),
				Actions.GetShouldSerializeMesh());
		}

		internal DeserializationRoute GetNewDeserializationRoute() {
			return new DeserializationRoute(
				Actions.GetShouldDeserializePathfinding(),
				Actions.GetShouldDeserializeMapPrefab(),
				Actions.GetShouldDeserializeSpriteShape(),
				Actions.GetShouldDeserializeColliderCoords(),
                Actions.GetShouldDeserializeMesh());
		}

		internal SerializedPrimitiveCollisionSolverCtx GetNewSerializedPrimitiveCollisionSolverCtx() {
			return new SerializedPrimitiveCollisionSolverCtx(
				Actions.GetColliderGameObject(),
				Actions.GetSkinWidth(),
				Actions.GetEdgeColliderRadius());
		}
	}
}