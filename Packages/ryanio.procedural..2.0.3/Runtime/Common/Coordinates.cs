// ProceduralGeneration

using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct Coordinates {
		internal Dictionary<int, List<Vector3>> SpriteBoundaryCoords { get; }
		internal Dictionary<int, List<Vector3>> ColliderCoords       { get; }

		public Coordinates(Dictionary<int, List<Vector3>> spriteBoundaryCoords,
			Dictionary<int, List<Vector3>> colliderCoords) {
			SpriteBoundaryCoords = spriteBoundaryCoords;
			ColliderCoords       = colliderCoords;
		}
	}
}