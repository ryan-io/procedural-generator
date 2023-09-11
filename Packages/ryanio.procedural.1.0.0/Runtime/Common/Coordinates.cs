// ProceduralGeneration

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct Coordinates {
		internal IReadOnlyDictionary<int, List<Vector2>> Outlines   { get; }

		internal Dictionary<int, List<Vector2>> ProcessedCoordsCoordsMutable
			=> Outlines.ToDictionary(key => key.Key, value => value.Value);

		public Coordinates(
			IReadOnlyDictionary<int, List<Vector2>> outlines) {
			Outlines   = outlines;
		}
	}
}