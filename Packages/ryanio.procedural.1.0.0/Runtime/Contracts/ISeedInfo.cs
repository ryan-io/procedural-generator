// Engine.Procedural

namespace ProceduralGeneration {
	public interface ISeed {
		string   CurrentSerializableName { get; }
		SeedInfo GetSeedInfo();
	}
}