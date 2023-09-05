// Engine.Procedural

using Unity.Mathematics;

namespace ProceduralGeneration {
	public readonly struct MapDimensionsIncludeCellSize {
		int MapWidth  { get; }
		int MapHeight { get; }

		public int2 Get() {
			var width  = Constants.Instance.CellSize * MapWidth;
			var height = Constants.Instance.CellSize * MapHeight;
			return new int2(width, height);
		}

		public MapDimensionsIncludeCellSize(Dimensions dimensions) {
			MapWidth  = dimensions.Rows;
			MapHeight = dimensions.Columns;
		}
	}
}