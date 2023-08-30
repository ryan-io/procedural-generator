// ProceduralGeneration

namespace ProceduralGeneration {
	internal class SerializationRouter {
		IActions            Actions                       { get; }
		SeedInfo            SeedInfo                      { get; }
		GeneratorSerializer Serializer                    { get; }
		string              SerializedName                { get; }
		bool                ShouldSerializePathfinding    { get; }
		bool                ShouldSerializeMapPrefab      { get; }
		bool                ShouldSerializeSpriteShape    { get; }
		bool                ShouldSerializeColliderCoords { get; }
		bool                ShouldSerializeMesh           { get; }


		internal void Run(string mapName, Coordinates coordinates) {
			Serializer.SerializeSeed(SeedInfo, mapName);
			var directories = DirectoryAction.GetMapDirectories(SerializedName);

			if (ShouldSerializeMesh)
				Serializer.SerializeMesh(
					new SerializeMeshJob(SerializedName, directories, Actions.GetMeshData().Mesh));

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


		internal SerializationRouter(SerializationRouterCtx ctx, SerializationRoute route, IActions actions) {
			Serializer = new GeneratorSerializer(ctx.Grid.gameObject, actions);

			Actions        = actions;
			SeedInfo       = ctx.SeedInfo;
			SerializedName = ctx.SerializableName;

			ShouldSerializePathfinding    = route.ShouldSerializePathfinding;
			ShouldSerializeMapPrefab      = route.ShouldSerializeMapPrefab;
			ShouldSerializeSpriteShape    = route.ShouldSerializeSpriteShape;
			ShouldSerializeColliderCoords = route.ShouldSerializeColliderCoords;
			ShouldSerializeMesh           = route.ShouldSerializeMesh;
		}
	}
}