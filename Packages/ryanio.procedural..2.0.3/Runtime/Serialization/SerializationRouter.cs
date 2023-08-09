// ProceduralGeneration

using System.Collections.Generic;
using BCL;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class SerializationRouter {
		GeneratorSerializer Serializer                    { get; }
		bool                ShouldSerializePathfinding    { get; }
		bool                ShouldSerializeMapPrefab      { get; }
		bool                ShouldSerializeSpriteShape    { get; }
		bool                ShouldSerializeColliderCoords { get; }

		public void ValidateAndSerialize(
			string name,
			string directory,
			Dictionary<int, List<SerializableVector3>> boundaryCoords,
			Dictionary<int, List<SerializableVector3>> colliderCoords) {
			var directories = DirectoryAction.GetMapDirectories(name);

			if (ShouldSerializePathfinding)
				Serializer.SerializeCurrentAstarGraph(name, directory);

			if (ShouldSerializeMapPrefab)
				Serializer.SerializeMapGameObject(name, directories);

			if (ShouldSerializeSpriteShape)
				Serializer.SerializeSpriteShape(name, boundaryCoords, directories);

			if (ShouldSerializeColliderCoords)
				Serializer.SerializeColliderCoords(name, colliderCoords, directories);
		}

		public SerializationRouter(ProceduralConfig config, GameObject gridObj, StopWatchWrapper stopWatch) {
			Serializer                    = new GeneratorSerializer(config, gridObj, stopWatch);
			ShouldSerializePathfinding    = config.ShouldSerializePathfinding;
			ShouldSerializeMapPrefab      = config.ShouldSerializeMapPrefab;
			ShouldSerializeSpriteShape    = config.ShouldSerializeSpriteShape;
			ShouldSerializeColliderCoords = config.ShouldSerializeColliderCoords;
		}
	}
}