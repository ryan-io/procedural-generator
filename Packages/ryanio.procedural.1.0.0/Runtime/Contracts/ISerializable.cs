// ProceduralGeneration

namespace ProceduralGeneration {
	internal interface ISerializable {
		bool GetShouldSerializePathfinding();
		bool GetShouldSerializeMapPrefab();
		bool GetShouldSerializeSpriteShape();
		bool GetShouldSerializeColliderCoords();
		bool GetShouldSerializeMesh();
	}
}