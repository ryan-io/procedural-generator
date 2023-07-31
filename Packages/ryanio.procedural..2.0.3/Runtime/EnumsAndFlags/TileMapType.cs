using System;

namespace Engine.Procedural.Runtime {
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