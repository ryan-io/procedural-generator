// Engine.Procedural

using System;
using System.IO;
using BCL;
using Engine.Tools.Serializer;

namespace Engine.Procedural {
	public readonly struct SerializeSeedInfo {
		public void Serialize(
			SeedInfo seedInfo, SerializerSetup setup, string nameToAppend, StopWatchWrapper stopwatch) {
			if (string.IsNullOrWhiteSpace(nameToAppend)) return;

			var sanitizedName = Constants.SAVE_SEED_PREFIX +
			                    nameToAppend               +
			                    Constants.UNDERSCORE       +
			                    seedInfo.CurrentSeed       +
			                    Constants.UID              +
			                    seedInfo.LastIteration;

			Serializer.EnsureDirectoryExists(
				setup.SaveRoot + Constants.SERIALIZED_DATA_FOLDER_NAME, setup.SaveRoot);

			Serializer.EnsureFileExists(
				setup.SaveRoot + Constants.SERIALIZED_DATA_FOLDER_NAME,
				Constants.SEED_TRACKER_FILE_NAME,
				setup.FileFormat);

			var trackerPath =
				setup.SaveRoot                        +
				Constants.SERIALIZED_DATA_FOLDER_NAME +
				Constants.BACKSLASH                   +
				Constants.SEED_TRACKER_FILE_NAME      +
				Constants.TXT_FILE_TYPE;

			File.AppendAllText(trackerPath, sanitizedName + Environment.NewLine);

			GenLogging.LogWithTimeStamp(
				LogLevel.Normal,
				stopwatch.TimeElapsed,
				Message.UPDATED_SEED_TRACKER,
				Constants.SERIALIZE_SEED_CTX);
		}
	}
}