// ProceduralGeneration

namespace ProceduralGeneration {
	internal readonly struct SerializationRoute {
		internal bool ShouldSerializePathfinding    { get; }
		internal bool ShouldSerializeMapPrefab      { get; }
		internal bool ShouldSerializeSpriteShape    { get; }
		internal bool ShouldSerializeColliderCoords { get; }
		internal bool ShouldSerializeMesh           { get; }

		public SerializationRoute(
			bool shouldSerializePathfinding,
			bool shouldSerializeMapPrefab,
			bool shouldSerializeSpriteShape,
			bool shouldSerializeColliderCoords, 
			bool shouldSerializeMesh) {
			ShouldSerializePathfinding    = shouldSerializePathfinding;
			ShouldSerializeMapPrefab      = shouldSerializeMapPrefab;
			ShouldSerializeSpriteShape    = shouldSerializeSpriteShape;
			ShouldSerializeColliderCoords = shouldSerializeColliderCoords;
			ShouldSerializeMesh           = shouldSerializeMesh;
		}
	}
}