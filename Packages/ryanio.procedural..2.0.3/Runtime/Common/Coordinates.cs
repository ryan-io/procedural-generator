// ProceduralGeneration

using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct Coordinates {
		internal IReadOnlyDictionary<int, List<Vector3>> SpriteBoundaryCoords { get; }
		internal IReadOnlyDictionary<int, List<Vector3>> ColliderCoords       { get; }

		public Coordinates(
			IReadOnlyDictionary<int, List<Vector3>> spriteBoundaryCoords,
			IReadOnlyDictionary<int, List<Vector3>> colliderCoords) {
			SpriteBoundaryCoords = spriteBoundaryCoords;
			ColliderCoords       = colliderCoords;
		}
	}
}