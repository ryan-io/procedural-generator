using System;

namespace ProceduralGeneration {
	[Serializable]
	public enum TileMapType {
		Ground,
		Boundary,
		ForegroundOne,
		ForegroundTwo,
		ForegroundThree,
		PlatformEffector,
		PlatformNoEffector, 
		Obstacles
	}
}