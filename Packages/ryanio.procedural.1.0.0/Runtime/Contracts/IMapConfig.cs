// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal interface IMapConfig {
		Dimensions GetMapDimensions();
		Vector2Int GetCorridorWidth();
		int        GetWallFillPercentage();
		int        GetUpperNeighborLimit();
		int        GetLowerNeighborLimit();
		int        GetSmoothingIterations();
		int        GetWallRemoveThreshold();
		int        GetRoomRemoveThreshold();
		int        GetBorderSize();
	}
}