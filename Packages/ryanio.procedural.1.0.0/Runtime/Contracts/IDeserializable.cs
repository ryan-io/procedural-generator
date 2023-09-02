// ProceduralGeneration

namespace ProceduralGeneration {
	internal interface IDeserializable {
		bool GetShouldDeserializePathfinding();
		bool GetShouldDeserializeMapPrefab();
		bool GetShouldDeserializeSpriteShape();
		bool GetShouldDeserializeColliderCoords();
		bool GetShouldDeserializeMesh();
	}
}