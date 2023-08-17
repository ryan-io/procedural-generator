// ProceduralGeneration

using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///  This needs work... will need to be refactored and overall architecture needs to be rethought
	/// </summary>
	internal interface IActions : IProceduralLogging {
		GameObject        GetOwner();
		Dimensions        GetMapDimensions();
		GameObject        GetColliderGameObject();
		TileMapDictionary GetTilemapDictionary();
		ColliderType      GetColliderType();
		ColliderSolverType GetColliderSolverType();
		LayerMask         GetObstacleMask();
		float             GetGraphCollideDiameter();
		float             GetNodeSize();
		TileDictionary    GetTileDictionary();
		Grid              GetGrid();
		SeedInfo          GetSeed();
		TileHashset       GetTileHashset();
		MeshData    GetMeshData();
		string            GetSerializationName();
		string            GetDeserializationName();
		float             GetTimeElapsed();
		int               GetWallFillPercentage();
		int               GetUpperNeighborLimit();
		int               GetLowerNeighborLimit();
		int               GetSmoothingIterations();
		Vector2Int        GetCorridorWidth();
		int               GetWallRemoveThreshold();
		int               GetRoomRemoveThreshold();
		int               GetBorderSize();
		bool              GetShouldCreateTileLabels();

		void    SetColliderGameObject(GameObject o);
		void    SetTileMapDictionary(TileMapDictionary tileMapDictionary);
		void    SetGrid(Grid grid);
		void    SetMeshData(MeshData meshData);
		float   GetSkinWidth();
		Vector2 GetEdgeColliderOffset();
		float   GetEdgeColliderRadius();
	}
}