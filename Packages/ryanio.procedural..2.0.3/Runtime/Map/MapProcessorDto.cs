namespace ProceduralGeneration {
	public readonly struct MapProcessorDto {
		public ProceduralConfig Config { get; }

		public int[,] Map { get; }

		public MapProcessorDto(ProceduralConfig config, int[,] map) {
			Config = config;
			Map           = map;
		}
	}
}