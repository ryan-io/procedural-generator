// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct GridCharacteristicsSolverCtx {
		internal Grid   Grid              { get; }
		internal string SerializationName { get; }

		public GridCharacteristicsSolverCtx(Grid grid, string serializationName) {
			Grid              = grid;
			SerializationName = serializationName;
		}
	}
}