// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct RemoveRegionsSolverCtx {
		internal Dimensions Dimensions          { get; }
		internal Vector2Int CorridorWidth       { get; }
		internal int        WallRemoveThreshold { get; }
		internal int        RoomRemoveThreshold { get; }


		internal RemoveRegionsSolverCtx(
			Dimensions dimensions,
			Vector2Int corridorWidth,
			int wallRemoveThreshold,
			int roomRemoveThreshold) {
			Dimensions          = dimensions;
			WallRemoveThreshold = wallRemoveThreshold;
			RoomRemoveThreshold = roomRemoveThreshold;
			CorridorWidth       = corridorWidth;
		}
	}
}