// ProceduralGeneration

using System.Collections.Generic;
using UnityBCL;

namespace ProceduralGeneration {
	internal class SerializationRouter {
		SeedInfo            SeedInfo                      { get; }
		GeneratorSerializer Serializer                    { get; }
		string              SerializedName                { get; }
		bool                ShouldSerializePathfinding    { get; }
		bool                ShouldSerializeMapPrefab      { get; }
		bool                ShouldSerializeSpriteShape    { get; }
		bool                ShouldSerializeColliderCoords { get; }

		internal void Run(string mapName, Coordinates coordinates) {
			var directory = new DirectoryAction().CreateNewDirectory(SerializedName);
			Serializer.SerializeSeed(SeedInfo, SerializedName);

			var directories = DirectoryAction.GetMapDirectories(mapName);
			if (ShouldSerializePathfinding)
				Serializer.SerializeCurrentAstarGraph(mapName, directory);

			if (ShouldSerializeMapPrefab)
				Serializer.SerializeMapGameObject(mapName, directories);

			if (ShouldSerializeSpriteShape) {
				var convertedCoords =
					new Convert().DictionaryVector3ToSerializedVector3(coordinates.SpriteBoundaryCoords);
				Serializer.SerializeSpriteShape(mapName, convertedCoords, directories);
			}

			if (ShouldSerializeColliderCoords) {
				var convertedCoords =
					new Convert().DictionaryVector3ToSerializedVector3(coordinates.ColliderCoords);
				Serializer.SerializeColliderCoords(mapName, convertedCoords, directories);
			}
		}

		internal SerializationRouter(SerializationRouterCtx ctx, SerializationRoute route, IProceduralLogging logger) {
			Serializer = new GeneratorSerializer(ctx.Grid.gameObject, logger);
			
			SeedInfo                      = ctx.SeedInfo;
			SerializedName                = ctx.SerializableName;
			
			ShouldSerializePathfinding    = route.ShouldSerializePathfinding;
			ShouldSerializeMapPrefab      = route.ShouldSerializeMapPrefab;
			ShouldSerializeSpriteShape    = route.ShouldSerializeSpriteShape;
			ShouldSerializeColliderCoords = route.ShouldSerializeColliderCoords;
		}
	}
}