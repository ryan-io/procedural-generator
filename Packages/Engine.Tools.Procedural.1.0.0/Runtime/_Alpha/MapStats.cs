using System;

namespace Engine.Procedural {
	[Serializable]
	public readonly struct MapStats {
		public MapStats(string seed, int iteration, string nameOfMap) {
			Seed      = seed;
			Iteration = iteration;
			NameOfMap = string.IsNullOrWhiteSpace(nameOfMap) ? "proceduralMap" : nameOfMap;
		}

		public int    Iteration { get; }
		public string NameOfMap { get; }
		public string Seed      { get; }
	}
}