// ProceduralGeneration

namespace ProceduralGeneration {
	internal readonly struct TileMapperCtx {
		internal bool ShouldCreateTileLabels { get; }

		public TileMapperCtx(bool shouldCreateTileLabels) {
			ShouldCreateTileLabels = shouldCreateTileLabels;
		}
	}
}