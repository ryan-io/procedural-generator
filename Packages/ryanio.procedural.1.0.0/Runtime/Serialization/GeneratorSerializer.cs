using System.Collections.Generic;
using System.Linq;
using BCL.Serialization;
using Pathfinding.Serialization;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	internal class GeneratorSerializer {
		GameObject         GridGo { get; }
		IProceduralLogging Logger { get; }

		/// <summary>
		/// This method should be invoked before invoking CreateMapFolder. This method will serialize the current seed.
		/// </summary>
		/// <param name="info">Info pertaining to current serialized seed</param>
		/// <param name="mapName">Name of map</param>
		internal void SerializeSeed(SeedInfo info, string mapName) {
			new SerializeSeedInfo().Serialize(info, mapName, Logger);
		}

		/// <summary>
		///  Serializes the current mesh to a file with the given name and path (relative to the project folder)
		/// </summary>
		internal void SerializeMesh(SerializeMeshJob job) {
			var serMesh = new SerializableMesh {
				Vertices  = job.Mesh.vertices.AsSerialized().ToArray(),
				Uvs       = job.Mesh.uv.AsSerialized().ToArray(),
				Triangles = job.Mesh.triangles
			};
			
			Help.ValidateNameIsSerialized(job.Name);

			var name = Constants.MESH_SAVE_PREFIX + job.Name;
			SerializeJson(name, job.Directories.full, serMesh);
		}

		/// <summary>
		///  Creates a folder for the map and returns the path to the folder as a string.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="directories"></param>
		internal void SerializeMapGameObject(string name, (string raw, string full) directories) {
			var mapDirectory  = new PathConstructor(directories);
			var path          = mapDirectory.GetUniquePathRawPrefab(Constants.SAVE_MAP_PREFIX + name);
			var prefabCreator = new PrefabCreator();

			if (!prefabCreator.CreateAndSaveAsPrefab(GridGo, path)) {
				Logger.LogError(Message.CANT_SERIALIZE_MAP_GO, nameof(SerializeMapGameObject));
			}
			else {
				Logger.Log(Message.SERIALIZE_MAP_AT + path, nameof(SerializeMapGameObject));
			}
		}

		/// <summary>
		///  Serializes the current A* graph to a file with the given name and path (relative to the project folder)
		/// </summary>
		/// <param name="name">Serializable map name</param>
		/// <param name="path"></param>
		internal void SerializeCurrentAstarGraph(string name, string path) {
			Help.ValidateNameIsSerialized(name);

			var settings = new SerializeSettings {
				nodes          = true,
				editorSettings = true
			};

			new SerializeAstar().Serialize(settings, name, path);
		}

		/// <summary>
		/// Serializes the current A* graph to a file with the given name and path (relative to the project folder)
		/// </summary>
		/// <param name="name">Name of A* graph file</param>
		/// <param name="coordinates">Collection of SerializableVector3</param>
		/// <param name="directories">Full and raw paths to SerializedData</param>
		internal void SerializeSpriteShape(
			string name,
			IReadOnlyDictionary<int, List<SerializableVector3>> coordinates,
			(string raw, string full) directories) {
			Help.ValidateNameIsSerialized(name);

			if (coordinates.IsEmptyOrNull())
				return;

			name = Constants.SPRITE_SHAPE_SAVE_PREFIX + name;
			SerializeJson(name, directories.full, coordinates);
		}

		/// <summary>
		/// Serializes a collection of SerializableVector3 to a file with the given name and path (relative to the project folder)
		/// </summary>
		/// <param name="name">Name of ColliderCoords file</param>
		/// <param name="coordinates">Collection of SerializableVector3</param>
		/// <param name="directories"></param>
		internal void SerializeColliderCoords(string name,
			IReadOnlyDictionary<int, List<SerializableVector3>> coordinates,
			(string raw, string full) directories) {
			if (string.IsNullOrWhiteSpace(name) || coordinates.IsEmptyOrNull())
				return;
			Help.ValidateNameIsSerialized(name);

			if (coordinates.IsEmptyOrNull())
				return;

			name = Constants.COLLIDER_COORDS_SAVE_PREFIX + name;
			SerializeJson(name, directories.full, coordinates);
		}

		/// <summary>
		/// Serializes an object to JSON with the given name and path (relative to the project folder)
		/// This method is used for serializing the coordinates of the map's collider.
		/// The name of the file will be prefixed with "ColliderCoords_".
		/// </summary>
		/// <param name="name">Name of file (map)</param>
		/// <param name="path">Save to directory</param>
		/// <param name="obj">The object to serialize</param>
		internal void SerializeJson(string name, string path, object obj) {
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path) || obj == default)
				return;

			var saveName   = name;
			var serializer = new Serializer(new UnityLogging());
			var job        = new SerializeJob.Json(saveName, path);

			serializer.SerializeAndSaveJson(obj, job);
		}

		/// <summary>
		///  Constructor for the GeneratorSerializer class.
		/// </summary>
		/// <param name="gridGo">Root GameObject containing ProceduralGenerator component</param>
		/// <param name="logger"></param>
		internal GeneratorSerializer(GameObject gridGo, IProceduralLogging logger) {
			GridGo = gridGo;
			Logger = logger;
		}
	}
}