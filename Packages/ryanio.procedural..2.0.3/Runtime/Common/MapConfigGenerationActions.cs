// ProceduralGeneration

using Pathfinding;
using UnityEngine;

namespace ProceduralGeneration {
	internal partial class Actions {
		public Dimensions   GetMapDimensions()       => new(ProceduralConfig.Rows, ProceduralConfig.Columns);
		public int          GetWallFillPercentage()  => ProceduralConfig.WallFillPercentage;
		public int          GetUpperNeighborLimit()  => ProceduralConfig.UpperNeighborLimit;
		public int          GetLowerNeighborLimit()  => ProceduralConfig.LowerNeighborLimit;
		public int          GetSmoothingIterations() => ProceduralConfig.SmoothingIterations;
		public Vector2Int   GetCorridorWidth()       => ProceduralConfig.CorridorWidth;
		public int          GetWallRemoveThreshold() => ProceduralConfig.WallRemovalThreshold;
		public int          GetRoomRemoveThreshold() => ProceduralConfig.RoomRemovalThreshold;
		public int          GetBorderSize()          => ProceduralConfig.BorderSize;
		public ColliderType GetColliderType()        => ProceduralConfig.NavGraphCollisionType;

		public LayerMask GetObstacleMask() => ProceduralConfig.ObstacleLayerMask;

		public float GetGraphCollideDiameter() => ProceduralConfig.NavGraphCollisionDetectionDiameter;

		public float GetNodeSize() => ProceduralConfig.NavGraphNodeSize;
	}
}