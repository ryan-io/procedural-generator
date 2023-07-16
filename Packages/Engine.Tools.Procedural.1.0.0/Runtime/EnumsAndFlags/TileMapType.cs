using System;

namespace Engine.Procedural {
	[Serializable]
	public enum TileMapType {
		Ground,
		Boundary,
		ForegroundOne,
		ForegroundTwo,
		ForegroundThree,
		PlatformEffector,
		PlatformNoEffector
	}
}