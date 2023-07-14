namespace Engine.Procedural {
	public readonly struct BorderAndBoundsSolverModel {
		public int[,] MapBorder     { get; }
		public int    MapBorderSize { get; }

		public BorderAndBoundsSolverModel(MapSpans model, ProceduralConfig config) {
			MapBorder     = model.BorderMap;
			MapBorderSize = config.BorderSize;
		}
	}
}