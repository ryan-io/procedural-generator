// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal partial class GenerationActions {
		public Dimensions GetMapDimensions()       => new(ProceduralConfig.Rows, ProceduralConfig.Columns);
		public int        GetWallFillPercentage()  => ProceduralConfig.WallFillPercentage;
		public int        GetUpperNeighborLimit()  => ProceduralConfig.UpperNeighborLimit;
		public int        GetLowerNeighborLimit()  => ProceduralConfig.LowerNeighborLimit;
		public int        GetSmoothingIterations() => ProceduralConfig.SmoothingIterations;
		public Vector2Int GetCorridorWidth()       => ProceduralConfig.CorridorWidth;
		public int        GetWallRemoveThreshold() => ProceduralConfig.WallRemovalThreshold;
		public int        GetRoomRemoveThreshold() => ProceduralConfig.RoomRemovalThreshold;
	}
}