using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BCL;
using Pathfinding.Serialization;
using Standalone.Serialization;
using UnityEditor;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class ProceduralSerializer {
		SerializerSetup  SeedSetup        { get; }
		SerializerSetup  AstarSetup       { get; }
		SerializerSetup  MapSetup         { get; }
		SerializerSetup  SpriteShapeSetup { get; }
		StopWatchWrapper StopWatch        { get; }

		public static IEnumerable<string> GetAllSeeds(SerializerSetup seedSetup) {
			var lines = File.ReadAllLines(
				seedSetup.SaveLocation           +
				Constants.BACKSLASH              +
				Constants.SEED_TRACKER_FILE_NAME +
				Constants.TXT_FILE_TYPE);

			var newLines = new string[lines.Length];

			for (var i = 0; i < newLines.Length; i++) {
				var l = lines[i];
				newLines[i] = l.Replace(Constants.SAVE_SEED_PREFIX, "");
			}

			return newLines.AsEnumerable();
		}

		public void SerializeSeed(SeedInfo info, ProceduralConfig config) {
			new SerializeSeedInfo().Serialize(info, SeedSetup, config.Name, StopWatch);
		}

		public void SerializeMapGameObject(string name, ProceduralConfig config) {
#if UNITY_EDITOR || UNITY_STANDALONE
			// if (new ValidationSerializedName().Validate(name, SeedSetup)) {
			// 	throw new Exception(Message.NO_NAME_FOUND + config.NameSeedIteration);
			// }

			var container = config.TilemapContainer;

			// this invokes MapSetup.SaveLocation { get; } and ensures directory exists
			_ = MapSetup.SaveLocation;
			var location = Constants.ASSETS_FOLDER    +
			               MapSetup.SaveRootNameRaw   +
			               Constants.BACKSLASH        +
			               MapSetup.SaveFolderNameRaw +
			               Constants.BACKSLASH;

			var path = location + name + MapSetup.FileFormat;
			path = AssetDatabase.GenerateUniqueAssetPath(path);

			PrefabUtility.SaveAsPrefabAsset(container.gameObject, path, out var creationSuccess);
			if (!creationSuccess) {
				GenLogging.Instance.Log(
					"Could not save map prefab. There may be one already serialized.",
					"SerializeMap",
					LogLevel.Error);
			}
			else {
				GenLogging.Instance.Log(
					"Serialized map at: " + path,
					"SerializeMap");
			}
#endif
		}

		/// <summary>
		///     Please ensure that your active graph data has been scanned.
		/// </summary>
		public void SerializeCurrentAstarGraph(string name) {
			if (new ValidationSerializedName().Validate(name, SeedSetup)) {
				throw new Exception(Message.NO_NAME_FOUND + name);
			}

			var settings = new SerializeSettings {
				nodes          = true,
				editorSettings = true
			};

			var bytes            = AstarPath.active.data.SerializeGraphs(settings);
			var serializationJob = new Serializer.TxtJob(AstarSetup, name, bytes, AstarSetup.FileFormat);
			Serializer.SaveBytesData(serializationJob, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="config">The map name, string, and generation iteration as a string</param>
		public void DeserializeAstarGraph(string name, ProceduralConfig config) {
			if (string.IsNullOrWhiteSpace(name))
				return;

			var validationPath =
				AstarSetup.SaveLocation     +
				Constants.SAVE_ASTAR_PREFIX +
				name                        +
				Constants.TXT_FILE_TYPE;

			var isValid = File.Exists(validationPath);

			if (!isValid) {
				GenLogging.Instance.Log(
					"The provided id: " + name + ", could not be found",
					"DeserializeAstarData",
					LogLevel.Error);
				return;
			}

			var hasData = Serializer.TryLoadBytesData(validationPath, out var data);

			if (hasData) {
				new GetActiveAstarData().Retrieve();
				var scanner = new GraphScanner(default);
				var builder = new GridGraphBuilder(config);
				var rule    = new NavGraphRulesSolver(config);

				AstarPath.active.data.DeserializeGraphs(data);
				var gridGraph = AstarPath.active.data.gridGraph;

				builder.SetGraph(gridGraph);
				rule.ResetGridGraphRules(gridGraph);
				rule.SetGridGraphRules(gridGraph);

				scanner.ScanGraph(gridGraph);
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

		public void DeserializeSpriteShape(string name, SpriteShapeConfig config) {
			if (string.IsNullOrWhiteSpace(name))
				return;

			var validationPath =
				AstarSetup.SaveLocation     +
				Constants.SAVE_ASTAR_PREFIX +
				name                        +
				Constants.TXT_FILE_TYPE;

			var isValid = File.Exists(validationPath);

			if (!isValid) {
				GenLogging.Instance.Log(
					"The provided id: " + name + ", could not be found",
					"DeserializeAstarData",
					LogLevel.Error);
				return;
			}

			var hasData = Serializer.TryLoadBytesData(validationPath, out var data);

			var testData = new Vector3(3, 4, 1);
			var test = JsonUtility.ToJson(testData);
			Serializer.SaveJson(new Serializer.JsonJob(SpriteShapeSetup, "TestData", test));
		}

		public ProceduralSerializer(ProceduralConfig config, StopWatchWrapper stopWatch) {
			SeedSetup        = config.SeedInfoSerializer;
			AstarSetup       = config.PathfindingSerializer;
			MapSetup         = config.MapSerializer;
			SpriteShapeSetup = config.SpriteShapeSerializer;
			StopWatch        = stopWatch;
		}
	}
}