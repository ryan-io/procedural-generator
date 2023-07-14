using System;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public readonly struct MapGenerationData {
		public MapGenerationData(Vector2Int mapDimensions) => MapDimensions = mapDimensions;

		public Vector2Int MapDimensions { get; }
	}
}