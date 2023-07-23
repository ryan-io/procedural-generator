using UnityEngine;

namespace Engine.Procedural.Runtime {
	public readonly struct MapSolverModel {
		public int        MapWidth             { get; }
		public int        MapHeight            { get; }
		public int        SmoothingIterations  { get; }
		public int        WallRemovalThreshold { get; }
		public int        RoomRemovalThreshold { get; }
		public Vector2Int CorridorWidth        { get; }
		public int        WallFillPercentage   { get; }
		public int        UpperNeighborLimit   { get; }
		public int        LowerNeighborLimit   { get; }
		public int        Seed                 { get; }

		public MapSolverModel(ProceduralConfig config) {
			MapWidth             = config.Rows;
			MapHeight            = config.Columns;
			WallFillPercentage   = config.WallFillPercentage;
			SmoothingIterations  = config.SmoothingIterations;
			UpperNeighborLimit   = config.UpperNeighborLimit;
			LowerNeighborLimit   = config.LowerNeighborLimit;
			Seed                 = config.Seed.GetHashCode();
			WallRemovalThreshold = config.WallRemovalThreshold;
			RoomRemovalThreshold = config.RoomRemovalThreshold;
			CorridorWidth        = config.CorridorWidth;
		}
	}
}