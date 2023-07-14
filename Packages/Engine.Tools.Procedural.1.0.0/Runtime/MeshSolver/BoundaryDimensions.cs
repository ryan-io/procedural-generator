namespace Engine.Procedural {
	public readonly struct BoundaryDimensions {
		public int Height     { get; }
		public int Width      { get; }
		public int BorderSize { get; }
		public int CellSize   { get; }

		public BoundaryDimensions(int height, int width, int cellSize, int borderSize) {
			Height     = height;
			Width      = width;
			CellSize   = cellSize;
			BorderSize = borderSize;
		}
	}
}