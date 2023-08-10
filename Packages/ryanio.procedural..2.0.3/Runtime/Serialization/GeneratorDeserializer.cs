// ProceduralGeneration

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BCL;
using BCL.Serialization;
using JetBrains.Annotations;
using UnityBCL;
using UnityEditor;
using UnityEngine;

namespace ProceduralGeneration {
	public class GeneratorDeserializer {
		ProceduralConfig Config     { get; }
		StopWatchWrapper StopWatch  { get; }
		Serializer       Serializer { get; set; }

		/// <summary>
		/// Takes the current map name, reads serialized data if found, and builds and scans a new A* graph
		/// </summary>
		/// <param name="currentSerializableName">Passed from an instance of ProceduralGenerator</param>
		/// <param name="mapDirectory">Directory relative to currentSerializableName</param>
		public void DeserializeAstar(string currentSerializableName, string mapDirectory) {
			if (string.IsNullOrWhiteSpace(currentSerializableName)) {
				GenLogging.Instance.Log("Serializable name was null or empty.", "DeserializeAstar", LogLevel.Warning);
				return;
			}

			Serializer ??= new Serializer();

			mapDirectory =
				mapDirectory                     +
				Constants.ASTAR_SERIALIZE_PREFIX +
				currentSerializableName          +
				Constants.JSON_FILE_TYPE;

			var isValid = File.Exists(mapDirectory);

			if (!isValid) {
				GenLogging.Instance.Log(
					"The provided id: " + currentSerializableName +
					", could not be found. This is likely due to not serializing Astar data (check settings).",
					"DeserializeAstarData",
					LogLevel.Error);
				return;
			}

			var data = Serializer.DeserializeJson<byte[]>(mapDirectory);

			if (!data.IsEmptyOrNull()) {
				new GetActiveAstarData().Retrieve();
				AstarPath.active.data.DeserializeGraphs(data);


				// scanning graph after deserializing it has been causing issues due to WalkabilityRule that is 
				// present

				//var scanner = new GraphScanner(StopWatch);
				//scanner.ScanGraph(AstarPath.active.data.gridGraph);
			}
			else {
				GenLogging.Instance.LogWithTimeStamp(
					LogLevel.Warning,
					StopWatch.TimeElapsed,
					Message.CANNOT_GET_SERIALIZED_ASTAR_DATA + mapDirectory,
					Constants.DESERIALIZE_ASTAR_CTX
				);
			}
		}

		/// <summary>
		///  Takes the current map name, reads serialized data if found, and generates a new sprite shape
		///  based on already serialized data
		/// </summary>
		/// <param name="currentSerializableName"></param>
		/// <param name="prefix"></param>
		/// <param name="directories"></param>
		/// <returns>Dictionary of outlines. Each key contains a list of coordinates that signify a 'room'</returns>
		/// <exception cref="Exception">Throws if no serialized data is found</exception>
		[CanBeNull]
		public Dictionary<int, List<Vector3>> DeserializeVector3(string currentSerializableName, string prefix,
			(string raw, string full) directories) {
			Serializer ??= new Serializer();

			var pathConstructor = new PathConstructor(directories);

			var validationPath =
				pathConstructor.GetUniquePathJson(prefix + currentSerializableName);

			var isValid = File.Exists(validationPath);
			
			if (!isValid) {
				GenLogging.Instance.Log(
					"The provided id: " + currentSerializableName +
					", could not be found. This is likely due to not serializing Sprite Shape border (check settings).",
					"DeserializeSpriteShape",
					LogLevel.Error);
				return default;
			}
			
			var dict = new Dictionary<int, List<Vector3>>();
			var serializedOutput =
				Serializer.DeserializeJson<Dictionary<int, List<SerializableVector3>>>(validationPath);

			if (serializedOutput.IsEmptyOrNull())
				return default;

			foreach (var pair in serializedOutput) {
				var list = pair.Value.Select(v => (Vector3)v).ToList();
				dict.Add(pair.Key, list);
			}

			return dict;
		}

		// [CanBeNull]
		// public Dictionary<int, List<Vector3>> DeserializeColliderCoords(string currentSerializableName, 
		// 	(string raw, string full) directories) {
		// 	Serializer ??= new Serializer();
		//
		// 	var pathConstructor = new PathConstructor(directories);
		//
		// 	var validationPath =
		// 		pathConstructor.GetUniquePathJson(Constants.COLLIDER_COORDS_SAVE_PREFIX + currentSerializableName);
		//
		// 	var isValid = File.Exists(validationPath);
		//
		// 	if (!isValid) {
		// 		GenLogging.Instance.Log(
		// 			"The provided id: " + currentSerializableName +
		// 			", could not be found. This is likely due to not serializing the Collider boundary points (check settings).",
		// 			"DeserializeColliderCoords",
		// 			LogLevel.Error);
		// 		return default;
		// 	}
		//
		// 	var dict = new Dictionary<int, List<Vector3>>();
		// 	var serializedOutput =
		// 		Serializer.DeserializeJson<Dictionary<int, List<SerializableVector3>>>(validationPath);
		//
		// 	if (serializedOutput.IsEmptyOrNull())
		// 		return default;
		//
		// 	foreach (var pair in serializedOutput) {
		// 		var list = pair.Value.Select(v => (Vector3)v).ToList();
		// 		dict.Add(pair.Key, list);
		// 	}
		//
		// 	return dict;
		// }

		[CanBeNull]
		public GameObject DeserializeMapPrefab(string currentSerializableName, (string raw, string full) directories) {
			var pathConstructor = new PathConstructor(directories);

			var validationPath =
				pathConstructor.GetUniquePathPrefab(Constants.SAVE_MAP_PREFIX + currentSerializableName);

			var assetPath =
				pathConstructor.GetUniquePathRawPrefab(Constants.SAVE_MAP_PREFIX + currentSerializableName);

			var isValid = File.Exists(validationPath);

			if (!isValid) {
				GenLogging.Instance.Log(
					"The provided id: " + currentSerializableName + ", could not be found. " +
					"This is likely due to not serializing the Map game object (check settings).",
					"DeserializeMap",
					LogLevel.Error);
				return default;
			}

			var obj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
			if (obj)
				GenLogging.Instance.Log($"Deserialize map prefab: {obj.name}", "DeserializeMap");
			return obj;
		}

		public GeneratorDeserializer(ProceduralConfig config, StopWatchWrapper stopWatch) {
			Serializer = new Serializer(new UnityLogging());
			Config     = config;
			StopWatch  = stopWatch;
		}
	}
}