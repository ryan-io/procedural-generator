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

#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif

namespace Engine.Procedural.Runtime {
	public class GeneratorSerializer {
		GameObject       Container           { get; }
		SerializerSetup  SeedSetup           { get; }
		SerializerSetup  AstarSetup          { get; }
		SerializerSetup  MapSetup            { get; }
		SerializerSetup  SpriteShapeSetup    { get; }
		SerializerSetup  ColliderCoordsSetup { get; }
		StopWatchWrapper StopWatch           { get; }

		public static IEnumerable<string> GetAllSeeds(SerializerSetup seedSetup) {
			var path = seedSetup.SaveLocation           +
			           Constants.BACKSLASH              +
			           Constants.SEED_TRACKER_FILE_NAME +
			           Constants.TXT_FILE_TYPE;
			;
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

		public void SerializeSeed(SeedInfo info, ProceduralConfig config) {
			new SerializeSeedInfo().Serialize(info, SeedSetup, config.Name, StopWatch);
		}

		public void SerializeMapGameObject(string name, ProceduralConfig config) {
#if UNITY_EDITOR || UNITY_STANDALONE
			// if (new ValidationSerializedName().Validate(name, SeedSetup)) {
			// 	throw new Exception(Message.NO_NAME_FOUND + config.NameSeedIteration);
			// }

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

			var serializer       = new Serializer();
			var bytes            = AstarPath.active.data.SerializeGraphs(settings);
			var serializationJob = new SerializeJob.Json(name, AstarSetup.SaveLocation);
			serializer.SerializeAndSaveJson(bytes, serializationJob);
		}

		public void SerializeSpriteShape(string name, Dictionary<int, List<SerializableVector3>> coordinates) {
			if (string.IsNullOrWhiteSpace(name) || coordinates.IsEmptyOrNull())
				return;

			var saveName = Constants.SPRITE_SHAPE_SAVE_PREFIX + name;
			SerializeJson(saveName, SpriteShapeSetup.SaveLocation, coordinates);
		}

		public void SerializeColliderCoords(string name, Dictionary<int, List<SerializableVector3>> coordinates) {
			if (string.IsNullOrWhiteSpace(name) || coordinates.IsEmptyOrNull())
				return;

			var saveName = Constants.COLLIDER_COORDS_SAVE_PREFIX + name;
			SerializeJson(saveName, ColliderCoordsSetup.SaveLocation, coordinates);
		}

		public void SerializeJson(string name, string path, object obj) {
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path) || obj == default)
				return;

			var saveName   = name;
			var serializer = new Serializer(new UnityLogging());
			var job        = new SerializeJob.Json(saveName, path);

			serializer.SerializeAndSaveJson(obj, job);
		}

		public GeneratorSerializer(ProceduralConfig config, GameObject container, StopWatchWrapper stopWatch) {
			SeedSetup           = config.SeedInfoSerializer;
			AstarSetup          = config.PathfindingSerializer;
			MapSetup            = config.MapSerializer;
			SpriteShapeSetup    = config.SpriteShapeSerializer;
			ColliderCoordsSetup = config.ColliderCoordsSerializer;
			StopWatch           = stopWatch;
			Container           = container;
		}
	}
}