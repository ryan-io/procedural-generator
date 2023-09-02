using System;

namespace ProceduralGeneration {
	[Serializable]
	public class TileId {
		public const string GROUND                = "Ground";
		public const string BOUNDARY              = "Boundary";
		public const string NORTH_EAST_ANGLE      = "NorthEastAngle";
		public const string NORTH_WEST_ANGLE      = "NorthWestAngle";
		public const string SOUTH_EAST_ANGLE      = "SouthEastAngle";
		public const string SOUTH_WEST_ANGLE      = "SouthWestAngle";
		public const string NORTH_POCKET          = "NorthPocket";
		public const string SOUTH_POCKET          = "SouthPocket";
		public const string EAST_POCKET           = "EastPocket";
		public const string WEST_POCKET           = "WestPocket";
		public const string SOUTH_OUTLINE         = "SouthOutline";
		public const string FOLIAGE_SOUTH_OUTLINE = "FoliageSouthOutline";
	}
}