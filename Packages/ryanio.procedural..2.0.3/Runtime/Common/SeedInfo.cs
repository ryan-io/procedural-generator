// Engine.Procedural

namespace Engine.Procedural.Runtime {
	public record SeedInfo(string Seed, int Iteration) {
		public string Seed   { get; } = Seed;
		public int    Iteration { get; } = Iteration;
	}
}