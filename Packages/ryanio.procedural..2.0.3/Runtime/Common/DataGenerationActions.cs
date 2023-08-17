// ProceduralGeneration

namespace ProceduralGeneration {
	internal partial class Actions {
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
		
		public MeshData GetMeshData() => MeshData;
	}
}