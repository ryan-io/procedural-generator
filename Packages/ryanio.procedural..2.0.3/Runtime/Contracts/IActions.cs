// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	public interface IActions : IProceduralLogging {
		GameObject                      GetOwner();
		ProceduralGeneration.Dimensions GetMapDimensions();
		GameObject                      GetColliderGameObject();
		TileMapDictionary               GetTilemapDictionary();
		Grid                            GetGrid();
		SeedInfo                        GetSeed();
		TileHashset                     GetTileHashset();
		string                          GetSerializationName();
		string                          GetDeserializationName();
		float                           GetTimeElapsed();
		int                             GetWallFillPercentage();
		int                             GetUpperNeighborLimit();
		int                             GetLowerNeighborLimit();
		int                             GetSmoothingIterations();
		Vector2Int                      GetCorridorWidth();
		int                             GetWallRemoveThreshold();
		int                             GetRoomRemoveThreshold();

		void SetColliderGameObject(GameObject o);
		void SetTileMapDictionary(TileMapDictionary tileMapDictionary);
		void SetGrid(Grid grid);
	}
}