// ProceduralGeneration

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct Coordinates {
		internal IReadOnlyDictionary<int, List<Vector3>> ProcessedCoords   { get; }
		internal IReadOnlyDictionary<int, List<Vector3>> UnprocessedCoords { get; }

		internal Dictionary<int, List<Vector3>> ProcessedCoordsCoordsMutable
			=> ProcessedCoords.ToDictionary(key => key.Key, value => value.Value);

		internal Dictionary<int, List<Vector3>> UnprocessedCoordsMutable
			=> UnprocessedCoordsMutable.ToDictionary(key => key.Key, value => value.Value);

		public Coordinates(
			IReadOnlyDictionary<int, List<Vector3>> processedCoords,
			IReadOnlyDictionary<int, List<Vector3>> unprocessed) {
			ProcessedCoords   = processedCoords;
			UnprocessedCoords = unprocessed;
		}
	}
}