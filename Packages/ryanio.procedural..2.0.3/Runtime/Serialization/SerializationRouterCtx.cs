// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct SerializationRouterCtx {
		internal SeedInfo SeedInfo         { get; }
		internal Grid     Grid             { get; }
		internal string   SerializableName { get; }

		public SerializationRouterCtx(SeedInfo seedInfo, Grid grid, string serializableName) {
			SeedInfo         = seedInfo;
			Grid             = grid;
			SerializableName = serializableName;
		}
	}
}