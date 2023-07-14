// Engine.Procedural

using Unity.Mathematics;

namespace Engine.Procedural {
	public readonly struct RealMapDimensions {
		int MapWidth  { get; }
		int MapHeight { get; }

		public int2 Get() {
			var width  = Constants.CELL_SIZE * MapWidth;
			var height = Constants.CELL_SIZE * MapHeight;
			return new int2(width, height);
		}

		public RealMapDimensions(ProceduralConfig config) {
			MapWidth  = config.Width;
			MapHeight = config.Height;
		}
	}
}