// ProceduralGeneration

using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct SpriteShapeBorderCtx {
		internal SpriteShapeConfig                       Config         { get; }
		internal GameObject                              Owner          { get; }
		internal IReadOnlyDictionary<int, List<Vector3>> Coordinates    { get; }
		internal string                                  SerializedName { get; }

		public SpriteShapeBorderCtx(
			SpriteShapeConfig config,
			GameObject owner,
			IReadOnlyDictionary<int, List<Vector3>> coordinates, 
			string serializedName) {
			Config              = config;
			Owner               = owner;
			Coordinates         = coordinates;
			SerializedName = serializedName;
		}
	}
}