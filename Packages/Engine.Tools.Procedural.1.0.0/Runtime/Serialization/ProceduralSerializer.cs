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
		SerializerSetup SeedSetup  { get; }
		SerializerSetup AstarSetup { get; }
		SerializerSetup MapSetup   { get; }

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

		StopWatchWrapper StopWatch { get; }

		public void SerializeSeed(SeedInfo info, ProceduralConfig config) {
			new SerializeSeedInfo().Serialize(info, SeedSetup, config.Name, StopWatch);
		}

		public void SerializeMapGameObject(ProceduralConfig config) {
//#if UNITY_EDITOR

			var name      = config.NameSeedIteration;
			var container = config.TilemapContainer;
			var location = Constants.ASSETS_FOLDER    +
			               MapSetup.SaveRootNameRaw   +
			               Constants.BACKSLASH        +
			               MapSetup.SaveFolderNameRaw +
			               Constants.BACKSLASH;
			var allocateLocation = MapSetup.SaveLocation;
			var path             = location + name + MapSetup.FileFormat;
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
//#endif
		}

		/// <summary>
		///     Please ensure that your active graph data has been scanned.
		/// </summary>
		public void SerializeCurrentAstarGraph(SerializerSetup setup, string name) {
			if (string.IsNullOrWhiteSpace(name)) name = "DefaultAstar";

			var settings = new SerializeSettings {
				nodes          = true,
				editorSettings = true
			};

			var bytes            = AstarPath.active.data.SerializeGraphs(settings);
			var serializationJob = new Serializer.TxtJob(setup, name, bytes, setup.FileFormat);
			Serializer.SaveBytesData(serializationJob, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="config">The map name, string, and generation iteration as a string</param>
		public void DeserializeAstarGraph(ProceduralConfig config) {
			if (string.IsNullOrWhiteSpace(config.NameSeedIteration))
				return;

			var validationPath =
				AstarSetup.SaveLocation     +
				Constants.SAVE_ASTAR_PREFIX +
				config.NameSeedIteration    +
				Constants.TXT_FILE_TYPE;

			var isValid = File.Exists(validationPath);

			if (!isValid) {
				GenLogging.Instance.Log(
					"The provided id: " + config.NameSeedIteration + ", could not be found",
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

		public ProceduralSerializer(ProceduralConfig config, StopWatchWrapper stopWatch) {
			SeedSetup  = config.SeedInfoSerializer;
			AstarSetup = config.PathfindingSerializer;
			MapSetup   = config.MapSerializer;
			StopWatch  = stopWatch;
		}
	}
}