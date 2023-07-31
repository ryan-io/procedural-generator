// Engine.Procedural

namespace Engine.Procedural.Runtime {
	public record SeedInfo(string CurrentSeed, string LastSeed, int LastIteration) {
		public string CurrentSeed   { get; } = CurrentSeed;
		public string LastSeed      { get; } = LastSeed;
		public int    LastIteration { get; } = LastIteration;
	}
}