// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct GeneratorToolsCtx {
		internal Dimensions Dimensions { get; }
		internal Grid       Grid       { get; }

		public GeneratorToolsCtx(Dimensions dimensions, Grid grid) {
			Dimensions = dimensions;
			Grid       = grid;
		}
	}
}