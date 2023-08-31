// ProceduralGeneration

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BCL.Serialization;
using JetBrains.Annotations;
using UnityBCL;
using UnityEditor;
using UnityEngine;

namespace ProceduralGeneration {
	public class GeneratorDeserializer {
		ProceduralConfig   Config     { get; }
		Serializer         Serializer { get; set; }
		IProceduralLogging Logger     { get; }

		/// <summary>
		/// Takes the current map name, reads serialized data if found, and builds and scans a new A* graph
		/// </summary>
		/// <param name="currentSerializableName">Passed from an instance of ProceduralGenerator</param>
		/// <param name="mapDirectory">Directory relative to currentSerializableName</param>
		public void DeserializeAstar(string currentSerializableName, string mapDirectory) {
			if (string.IsNullOrWhiteSpace(currentSerializableName)) {
				Logger.LogWarning(Message.SERIALIZED_NAME_NULL_EMPTY, nameof(DeserializeAstar));
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
				Logger.LogError(
					Message.SERIALIZE_GENERAL_INVALID_NAME_PREFIX +
					currentSerializableName                       +
					Message.SERIALIZE_ASTAR_INVALID_NAME,
					nameof(DeserializeAstar));

				return;
			}

			var data = Serializer.DeserializeJson<byte[]>(mapDirectory);

			if (!data.IsEmptyOrNull()) {
				new ActiveAstarData().Retrieve();
				AstarPath.active.data.DeserializeGraphs(data);


				// scanning graph after deserializing it has been causing issues due to WalkabilityRule that is 
				// present

				//var scanner = new GraphScanner(StopWatch);
				//scanner.ScanGraph(AstarPath.active.data.gridGraph);
			}
			else {
				Logger.LogWarning(Message.CANNOT_GET_SERIALIZED_ASTAR_DATA + mapDirectory, nameof(DeserializeAstar));
			}
		}

		/// <summary>
		///  Takes the current map name, reads serialized data if found, and deserialize a new mesh from a
		///  serialized mesh
		/// </summary>
		/// <param name="currentSerializableName"></param>
		/// <param name="prefix"></param>
		/// <param name="directories"></param>
		/// <returns>Mesh</returns>
		[CanBeNull]
		public Mesh DeserializeMesh(string currentSerializableName, string prefix,
			(string raw, string full) directories) {
			Serializer ??= new Serializer();

			var pathConstructor = new PathConstructor(directories);

			var validationPath =
				pathConstructor.GetUniquePathJson(prefix + currentSerializableName);

			var isValid = File.Exists(validationPath);

			if (!isValid) {
				Logger.LogWarning(
					Message.SERIALIZE_GENERAL_INVALID_NAME_PREFIX +
					currentSerializableName                       +
					Message.SERIALIZE_SPRITE_SHAPE_INVALID_NAME,
					nameof(DeserializeVector3));

				return default;
			}

			var mesh             = new Mesh();
			var serializedOutput = Serializer.DeserializeJson<SerializableMesh>(validationPath);

			mesh.triangles = serializedOutput.Triangles;
			mesh.uv        = serializedOutput.Uvs.Deserialized().ToArray();
			mesh.vertices  = serializedOutput.Vertices.Deserialized().ToArray();
			mesh.name      = Constants.SAVE_MESH_PREFIX + currentSerializableName;
			
			mesh.RecalculateNormals();

			return mesh;
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
				Logger.LogWarning(
					Message.SERIALIZE_GENERAL_INVALID_NAME_PREFIX +
					currentSerializableName                       +
					Message.SERIALIZE_SPRITE_SHAPE_INVALID_NAME,
					nameof(DeserializeVector3));

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

		[CanBeNull]
		public GameObject DeserializeMapPrefab(string currentSerializableName, (string raw, string full) directories) {
			var pathConstructor = new PathConstructor(directories);

			var validationPath =
				pathConstructor.GetUniquePathPrefab(Constants.SAVE_MAP_PREFIX + currentSerializableName);

			var assetPath =
				pathConstructor.GetUniquePathRawPrefab(Constants.SAVE_MAP_PREFIX + currentSerializableName);

			var isValid = File.Exists(validationPath);

			if (!isValid) {
				Logger.LogWarning(
					Message.SERIALIZE_GENERAL_INVALID_NAME_PREFIX +
					currentSerializableName                       +
					Message.SERIALIZE_COLLIDER_COORDS_INVALID_NAME,
					nameof(DeserializeMapPrefab));

				return default;
			}

			// TODO: should be 'build-friendly' and not use AssetDatabase
#if UNITY_EDITOR
			var obj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

			if (obj) {
				Logger.Log(Message.DESERIALIZE_MAP_PREFAB + obj.name, nameof(DeserializeMapPrefab));
			return obj;
			}
#endif
			return default;
		}

		public GeneratorDeserializer(IProceduralLogging logger) {
			Serializer = new Serializer(new UnityLogging());
			Logger     = logger;
		}
	}
}