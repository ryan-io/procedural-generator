namespace ProceduralGeneration {
	public readonly struct MapDimensionsModel {
		public int MapWidth   { get; }
		public int MapHeight  { get; }
		public int BorderSize { get; }
		public int CellSize   { get; }

		public MapDimensionsModel(int mapWidth, int mapHeight, int borderSize, int cellSize) {
			MapWidth   = mapWidth;
			MapHeight  = mapHeight;
			BorderSize = borderSize;
			CellSize   = cellSize;
		}
	}
}