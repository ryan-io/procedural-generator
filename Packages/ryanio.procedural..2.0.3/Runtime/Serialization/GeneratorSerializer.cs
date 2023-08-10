using System.Collections.Generic;
using BCL.Serialization;
using Pathfinding.Serialization;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public class GeneratorSerializer {
		GameObject           Container           { get; }
		BCL.StopWatchWrapper StopWatch           { get; }

		/// <summary>
		/// This method should be invoked before invoking CreateMapFolder.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="config"></param>
		public void SerializeSeed(SeedInfo info, ProceduralConfig config) {
			new SerializeSeedInfo().Serialize(info, config.Name);
		}

		/// <summary>
		///  Creates a folder for the map and returns the path to the folder as a string.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="directories"></param>
		public void SerializeMapGameObject(string name, (string raw, string full) directories) {
			var mapDirectory  = new PathConstructor(directories);
			var path          = mapDirectory.GetUniquePathRawPrefab(Constants.SAVE_MAP_PREFIX + name);
			var prefabCreator = new PrefabCreator();

			if (!prefabCreator.CreateAndSaveAsPrefab(Container, path)) {
				GenLogging.Instance.Log(
					"Could not save map prefab. There may be one already serialized.",
					"SerializeMap",
					BCL.LogLevel.Error);
			}
			else {
				GenLogging.Instance.Log(
					"Serialized map at: " + path,
					"SerializeMap");
			}
		}

		/// <summary>
		///  Serializes the current A* graph to a file with the given name and path (relative to the project folder)
		/// </summary>
		/// <param name="name">Serializable map name</param>
		/// <param name="path"></param>
		public void SerializeCurrentAstarGraph(string name, string path) {
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
		public void SerializeSpriteShape(
			string name,
			Dictionary<int, List<SerializableVector3>> coordinates,
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
		public void SerializeColliderCoords(string name, Dictionary<int, List<SerializableVector3>> coordinates,
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
		public void SerializeJson(string name, string path, object obj) {
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
		/// <param name="config">ProcGen config</param>
		/// <param name="container">Root GameObject containing ProceduralGenerator component</param>
		/// <param name="stopWatch">Instance of stopwatch for outputting timeElapsed</param>
		public GeneratorSerializer(ProceduralConfig config, GameObject container, BCL.StopWatchWrapper stopWatch) {
			StopWatch           = stopWatch;
			Container           = container;
		}
	}
}