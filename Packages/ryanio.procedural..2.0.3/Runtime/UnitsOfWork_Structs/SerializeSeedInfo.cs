// Engine.Procedural

using System;
using System.IO;
using BCL.Serialization;
using UnityBCL.Serialization;

namespace ProceduralGeneration {
	public readonly struct SerializeSeedInfo {
		public void Serialize(SeedInfo seedInfo, string name) {
			if (string.IsNullOrWhiteSpace(name)) return;

			var content = name                 +
			              Constants.UNDERSCORE +
			              seedInfo.Seed        +
			              Constants.UID        +
			              seedInfo.Iteration;

			var location = UnitySaveLocation.GetDefault;

			var serializer = new Serializer();

			serializer.EnsureFileExists(
				location.SaveLocation,
				Constants.SEED_TRACKER_FILE_NAME,
				Constants.TXT_FILE_TYPE);

			var path = location.GetFilePath(
				Constants.SEED_TRACKER_FILE_NAME,
				Constants.TXT_FILE_TYPE);

			//
			// var trackerPath =
			// 	location.SaveLocation            +
			// 	Constants.BACKSLASH              +
			// 	Constants.SEED_TRACKER_FILE_NAME +
			// 	Constants.TXT_FILE_TYPE;

			File.AppendAllText(path, content + Environment.NewLine);

			GenLogging.Instance.Log(
				Message.UPDATED_SEED_TRACKER,
				Constants.SERIALIZE_SEED_CTX);
		}
	}
}