using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BCL;
using BCL.Serialization;
using Pathfinding.Serialization;
using UnityBCL;
using UnityBCL.Serialization;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif

namespace Engine.Procedural.Runtime {
	public class GeneratorSerializer {
		GameObject       Container           { get; }
		SerializerSetup  MapSetup            { get; }
		SerializerSetup  SpriteShapeSetup    { get; }
		SerializerSetup  ColliderCoordsSetup { get; }
		StopWatchWrapper StopWatch           { get; }

		/// <summary>
		///  Gets and reads text from seedTracker.txt file.
		///  If the file does not exist, an empty collection is returned.
		///  This is also used in inspectors that need to display all seeds or invoke logic based on the seeds.
		/// </summary>
		/// <returns>Enumerable of type string</returns>
		public static IEnumerable<string> GetAllSeeds() {
			var location = UnitySaveLocation.GetDefault;
			var path     = location.GetFilePath(Constants.SEED_TRACKER_FILE_NAME, Constants.TXT_FILE_TYPE);

			var hasFile = File.Exists(path);

			if (!hasFile)
				return Enumerable.Empty<string>();

			var lines = File.ReadAllLines(path);

			if (lines.IsEmptyOrNull())
				return Enumerable.Empty<string>();

			var newLines = new string[lines.Length];

			for (var i = 0; i < newLines.Length; i++) {
				var l = lines[i];
				newLines[i] = l.Replace(Constants.SAVE_SEED_PREFIX, "");
			}

			return newLines.AsEnumerable();
		}

		/// <summary>
		/// This method should be invoked before invoking CreateMapFolder.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="config"></param>
		public void SerializeSeed(SeedInfo info, ProceduralConfig config) {
			new SerializeSeedInfo().Serialize(info, config.Name);
		}

		/// <summary>
		///  
		/// </summary>
		/// <param name="name"></param>
		public void SerializeMapGameObject(string name) {
#if UNITY_EDITOR

			var path       = MapSetup.SaveLocation  + Constants.SAVE_MAP_PREFIX + name + MapSetup.FileFormat;
			var uniquePath = MapSetup.SaveFolderRaw + Constants.SAVE_MAP_PREFIX + name + MapSetup.FileFormat;
			uniquePath = AssetDatabase.GenerateUniqueAssetPath(uniquePath);
			PrefabUtility.SaveAsPrefabAsset(Container.gameObject, path, out var creationSuccess);

			if (!creationSuccess) {
				GenLogging.Instance.Log(
					"Could not save map prefab. There may be one already serialized.",
					"SerializeMap",
					LogLevel.Error);
			}
			else {
				// var settings = AddressableAssetSettingsDefaultObject.Settings;
				// var guid     = AssetDatabase.AssetPathToGUID(uniquePath);
				// settings.CreateAssetReference(guid);
				// var entry = settings.FindAssetEntry(guid);
				// entry.address = Constants.SAVE_MAP_PREFIX + name;

				GenLogging.Instance.Log(
					"Serialized map at: " + path,
					"SerializeMap");
			}
#endif
		}

		// Description: Serializes the current A* graph to a file with the given name
		// Parameters: name - the name of the file to save the graph to
		// Returns: void
		//
		// Example: https://arongranberg.com/astar/docs/_serialization_example_8cs_source.php
		public void SerializeCurrentAstarGraph(string name, string path) {
			ValidateNameIsSerialized(name);

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
		public void SerializeSpriteShape(string name, Dictionary<int, List<SerializableVector3>> coordinates) {
			if (string.IsNullOrWhiteSpace(name) || coordinates.IsEmptyOrNull())
				return;

			var saveName = Constants.SPRITE_SHAPE_SAVE_PREFIX + name;
			SerializeJson(saveName, SpriteShapeSetup.SaveLocation, coordinates);
		}

		/// <summary>
		/// Serializes a collection of SerializableVector3 to a file with the given name and path (relative to the project folder)
		/// </summary>
		/// <param name="name">Name of ColliderCoords file</param>
		/// <param name="coordinates">Collection of SerializableVector3</param>
		public void SerializeColliderCoords(string name, Dictionary<int, List<SerializableVector3>> coordinates) {
			if (string.IsNullOrWhiteSpace(name) || coordinates.IsEmptyOrNull())
				return;

			var saveName = Constants.COLLIDER_COORDS_SAVE_PREFIX + name;
			SerializeJson(saveName, ColliderCoordsSetup.SaveLocation, coordinates);
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
		public GeneratorSerializer(ProceduralConfig config, GameObject container, StopWatchWrapper stopWatch) {
			MapSetup            = config.MapSerializer;
			SpriteShapeSetup    = config.SpriteShapeSerializer;
			ColliderCoordsSetup = config.ColliderCoordsSerializer;
			StopWatch           = stopWatch;
			Container           = container;
		}

		/// <summary>
		///  Helper method for validating that the given name is serialized.
		/// </summary>
		/// <param name="name">Name to serialize</param>
		/// <exception cref="Exception">Throws if name is not serialized</exception>
		static void ValidateNameIsSerialized(string name) {
			var isSerialized = new ValidationSerializedName().Validate(name);

			if (!isSerialized) {
				throw new Exception(Message.NO_NAME_FOUND + name);
			}
		}
	}
}