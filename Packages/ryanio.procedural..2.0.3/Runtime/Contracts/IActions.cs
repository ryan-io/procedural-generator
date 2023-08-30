// ProceduralGeneration

using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///  This needs work... will need to be refactored and overall architecture needs to be rethought
	/// </summary>
	internal interface IActions : IProceduralLogging {
		GameObject                         GetOwner();
		Coordinates                        GetCoordinates();
		Dimensions                         GetMapDimensions();
		GameObject                         GetColliderGameObject();
		SpriteShapeConfig                  GetSpriteShapeConfig();
		TileMapDictionary                  GetTilemapDictionary();
		IReadOnlyList<GraphColliderCutter> GetGraphColliderCutters();
		ColliderType                       GetColliderType();
		ColliderSolverType                 GetColliderSolverType();
		LayerMask                          GetObstacleMask();
		float                              GetGraphCollideDiameter();
		float                              GetNodeSize();
		TileDictionary                     GetTileDictionary();
		Grid                               GetGrid();
		SeedInfo                           GetSeed();
		TileHashset                        GetTileHashset();
		MeshData                           GetMeshData();
		string                             GetSerializationName();
		string                             GetDeserializationName();
		float                              GetTimeElapsed();
		int                                GetWallFillPercentage();
		int                                GetUpperNeighborLimit();
		int                                GetLowerNeighborLimit();
		int                                GetSmoothingIterations();
		Vector2Int                         GetCorridorWidth();
		int                                GetWallRemoveThreshold();
		int                                GetRoomRemoveThreshold();
		int                                GetBorderSize();
		bool                               GetShouldCreateTileLabels();
		bool                               GetShouldSerializePathfinding();
		bool                               GetShouldSerializeMapPrefab();
		bool                               GetShouldSerializeSpriteShape();
		bool                               GetShouldSerializeColliderCoords();
		bool                               GetShouldSerializeMesh();
		float                              GetSkinWidth();
		Vector2                            GetEdgeColliderOffset();
		float                              GetEdgeColliderRadius();

		void                SetColliderGameObject(GameObject o);
		void                SetTileMapDictionary(TileMapDictionary tileMapDictionary);
		void                SetGrid(Grid grid);
		void                SetCoords(Coordinates coordinates);
		void                SetMeshData(MeshData meshData);
		void                StopTimer();
		string              GetMapName();
		bool                GetShouldDeserializePathfinding();
		bool                GetShouldDeserializeMapPrefab();
		bool                GetShouldDeserializeSpriteShape();
		bool                GetShouldDeserializeColliderCoords();
		EventDictionary     GetSerializedEvents();
		IReadOnlyList<Room> GetRooms();
		void                SetRooms(IReadOnlyList<Room> rooms);
	}
}