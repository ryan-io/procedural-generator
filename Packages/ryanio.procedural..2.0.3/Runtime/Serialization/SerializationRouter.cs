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
			Serializer.SerializeSeed(SeedInfo, mapName);
			//var directory = new DirectoryAction().CreateNewDirectory(SerializedName);
			var directories = DirectoryAction.GetMapDirectories(SerializedName);
			
			if (ShouldSerializePathfinding)
				Serializer.SerializeCurrentAstarGraph(SerializedName, directories.full);

			if (ShouldSerializeMapPrefab)
				Serializer.SerializeMapGameObject(SerializedName, directories);

			if (ShouldSerializeSpriteShape) {
				var convertedCoords =
					new Convert().DictionaryVector3ToSerializedVector3(coordinates.ProcessedCoords);
				Serializer.SerializeSpriteShape(SerializedName, convertedCoords, directories);
			}

			if (ShouldSerializeColliderCoords) {
				var convertedCoords =
					new Convert().DictionaryVector3ToSerializedVector3(coordinates.ProcessedCoords);
				Serializer.SerializeColliderCoords(SerializedName, convertedCoords, directories);
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