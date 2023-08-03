// ProceduralGeneration

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BCL;
using BCL.Serialization;
using JetBrains.Annotations;
using UnityBCL;
using UnityBCL.Serialization;
using UnityEditor;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class GeneratorDeserializer {
		ProceduralConfig Config     { get; }
		StopWatchWrapper StopWatch  { get; }
		Serializer       Serializer { get; set; }
		
		/// <summary>
		/// Takes the current map name, reads serialized data if found, and builds and scans a new A* graph
		/// </summary>
		/// <param name="currentSerializableName">Passed from an instance of ProceduralGenerator</param>
		/// <param name="setup">SerializerSetup used for Astar</param>
		public void DeserializeAstar(string currentSerializableName, SerializerSetup setup) {
			if (string.IsNullOrWhiteSpace(currentSerializableName)) {
				GenLogging.Instance.Log("Serializable name was null or empty.", "DeserializeAstar", LogLevel.Warning);
				return;
			}

			Serializer = new Serializer();

			var validationPath =
				setup.SaveLocation          +
				Constants.SAVE_ASTAR_PREFIX +
				currentSerializableName     +
				Constants.JSON_FILE_TYPE;

			var isValid = File.Exists(validationPath);

			if (!isValid) {
				GenLogging.Instance.Log(
					"The provided id: " + currentSerializableName + ", could not be found",
					"DeserializeAstarData",
					LogLevel.Error);
				return;
			}

			var data = Serializer.DeserializeJson<byte[]>(validationPath);

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
					Message.CANNOT_GET_SERIALIZED_ASTAR_DATA + validationPath,
					Constants.DESERIALIZE_ASTAR_CTX
				);
			}
		}
		

		[CanBeNull]
		public Dictionary<int, List<Vector3>> DeserializeSpriteShape(
			string currentSerializableName) {
			try {
				Serializer ??= new Serializer();

				var validationPath =
					Config.SpriteShapeSerializer.SaveLocation +
					Constants.SPRITE_SHAPE_SAVE_PREFIX        +
					currentSerializableName                   +
					Constants.JSON_FILE_TYPE;

				var isValid = File.Exists(validationPath);

				if (!isValid) {
					throw new Exception(
						"File name was not valid. Could not validate if file '" +
						currentSerializableName                                 + "' exists");
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

			catch (Exception e) {
				GenLogging.Instance.Log(
					"The provided id: " + currentSerializableName + ", could not be found. Error: " + e.Message,
					"DeserializeAstarData",
					LogLevel.Error);
				throw;
			}
		}
		
		[CanBeNull]
		public Dictionary<int, List<Vector3>> DeserializeColliderCoords(string currentSerializableName) {
			try {
				Serializer ??= new Serializer();

				var validationPath =
					Config.ColliderCoordsSerializer.SaveLocation +
					Constants.COLLIDER_COORDS_SAVE_PREFIX        +
					currentSerializableName                   +
					Constants.JSON_FILE_TYPE;

				var isValid = File.Exists(validationPath);

				if (!isValid) {
					throw new Exception(
						"File name was not valid. Could not validate if file '" +
						currentSerializableName                                 + "' exists");
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

			catch (Exception e) {
				GenLogging.Instance.Log(
					"The provided id: " + currentSerializableName + ", could not be found. Error: " + e.Message,
					"DeserializeAstarData",
					LogLevel.Error);
				throw;
			}
		}

		public GameObject DeserializeMapPrefab(string currentSerializableName) {
			try {
				var validationPath =
					Config.MapSerializer.SaveLocation +
					Constants.SAVE_MAP_PREFIX         +
					currentSerializableName           +
					Constants.PREFAB_FILE_TYPE;

				var uniquePath = Constants.ASSETS_FOLDER            +
				                 Config.MapSerializer.SaveFolderRaw +
				                 Constants.BACKSLASH                +
				                 Constants.SAVE_MAP_PREFIX          +
				                 currentSerializableName            +
				                 Config.MapSerializer.FileFormat;

				var isValid = File.Exists(validationPath);


				if (!isValid) {
					throw new Exception(
						$"File name was not valid. Could not validate if file '{currentSerializableName}' exists");
				}

				var obj = AssetDatabase.LoadAssetAtPath<GameObject>(uniquePath);
				if (obj)
					GenLogging.Instance.Log($"Deserialize map prefab: {obj.name}", "DeserializeMap");
				return obj;
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
		}

		public GeneratorDeserializer(ProceduralConfig config, StopWatchWrapper stopWatch) {
			Serializer = new Serializer(new UnityLogging());
			Config     = config;
			StopWatch  = stopWatch;
		}
	}
}