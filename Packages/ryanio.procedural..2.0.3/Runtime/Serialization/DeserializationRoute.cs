// ProceduralGeneration

namespace ProceduralGeneration {
	internal readonly struct DeserializationRoute {
		internal bool ShouldDeserializePathfinding    { get; }
		internal bool ShouldDeserializeMapPrefab      { get; }
		internal bool ShouldDeserializeSpriteShape    { get; }
		internal bool ShouldDeserializeColliderCoords { get; }

		public DeserializationRoute(
			bool shouldDeserializePathfinding, 
			bool shouldDeserializeMapPrefab, 
			bool shouldDeserializeSpriteShape, 
			bool shouldDeserializeColliderCoords) {
			ShouldDeserializePathfinding    = shouldDeserializePathfinding;
			ShouldDeserializeMapPrefab      = shouldDeserializeMapPrefab;
			ShouldDeserializeSpriteShape    = shouldDeserializeSpriteShape;
			ShouldDeserializeColliderCoords = shouldDeserializeColliderCoords;
		}
	}
}