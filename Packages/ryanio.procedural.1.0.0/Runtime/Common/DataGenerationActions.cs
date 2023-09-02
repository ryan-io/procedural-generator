// ProceduralGeneration

namespace ProceduralGeneration {
	internal partial class Actions {
		Coordinates Coordinates { get; set; }
		
		public SeedInfo GetSeed() => new(ProceduralConfig.Seed, ProceduralConfig.LastIteration);
		
		public string GetSerializationName() {
			var seedInfo = GetSeed();

			return ProceduralConfig.Name +
			       Constants.UNDERSCORE  +
			       seedInfo.Seed         +
			       Constants.UID         +
			       seedInfo.Iteration;
		}

		public string GetDeserializationName() => ProceduralConfig.NameSeedIteration;

		public Coordinates GetCoordinates() => Coordinates;

		public void SetCoords(Coordinates coordinates) => Coordinates = coordinates;
		
		public MeshData GetMeshData() => MeshData;
	}
}