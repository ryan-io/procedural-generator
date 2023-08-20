// ProceduralGeneration

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct Coordinates {
		internal IReadOnlyDictionary<int, List<Vector3>> SpriteBoundaryCoords { get; }
		internal IReadOnlyDictionary<int, List<Vector3>> ColliderCoords       { get; }

		internal Dictionary<int, List<Vector3>> SpriteBoundaryCoordsMutable 
			=> SpriteBoundaryCoords.ToDictionary(key => key.Key, value => value.Value);
		
		internal Dictionary<int, List<Vector3>> ColliderCoordsMutable 
			=> ColliderCoords.ToDictionary(key => key.Key, value => value.Value);
		
		public Coordinates(
			IReadOnlyDictionary<int, List<Vector3>> spriteBoundaryCoords,
			IReadOnlyDictionary<int, List<Vector3>> colliderCoords) {
			SpriteBoundaryCoords = spriteBoundaryCoords;
			ColliderCoords       = colliderCoords;
		}
	}
}