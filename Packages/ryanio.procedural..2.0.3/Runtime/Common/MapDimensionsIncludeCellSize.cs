// Engine.Procedural

using Unity.Mathematics;

namespace Engine.Procedural.Runtime {
	public readonly struct MapDimensionsIncludeCellSize {
		int MapWidth  { get; }
		int MapHeight { get; }

		public int2 Get() {
			var width  = Constants.CELL_SIZE * MapWidth;
			var height = Constants.CELL_SIZE * MapHeight;
			return new int2(width, height);
		}

		public MapDimensionsIncludeCellSize(ProceduralConfig config) {
			MapWidth  = config.Rows;
			MapHeight = config.Columns;
		}
	}
}