// Engine.Procedural

namespace ProceduralGeneration {
	public interface ISeedInfo {
		string   CurrentSerializableName { get; }
		SeedInfo GetSeedInfo();
	}
}