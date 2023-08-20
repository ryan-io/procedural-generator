// ProceduralGeneration

namespace ProceduralGeneration {
	internal readonly struct SerializationRoute {
		internal bool ShouldSerializePathfinding    { get; }
		internal bool ShouldSerializeMapPrefab      { get; }
		internal bool ShouldSerializeSpriteShape    { get; }
		internal bool ShouldSerializeColliderCoords { get; }

		public SerializationRoute(
			bool shouldSerializePathfinding, 
			bool shouldSerializeMapPrefab, 
			bool shouldSerializeSpriteShape, 
			bool shouldSerializeColliderCoords) {
			ShouldSerializePathfinding    = shouldSerializePathfinding;
			ShouldSerializeMapPrefab      = shouldSerializeMapPrefab;
			ShouldSerializeSpriteShape    = shouldSerializeSpriteShape;
			ShouldSerializeColliderCoords = shouldSerializeColliderCoords;
		}
	}
}