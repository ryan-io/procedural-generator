// Engine.Procedural

namespace Engine.Procedural.Runtime {
	public interface ISeedInfo {
		string   CurrentSerializableName { get; }
		SeedInfo GetSeedInfo();
	}
}