// Engine.Procedural

using System;
using System.IO;
using BCL;
using BCL.Serialization;
using UnityBCL.Serialization;

namespace Engine.Procedural.Runtime {
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

			var serializer = new Serializer();
			
			serializer.EnsureFileExists(
				setup.SaveLocation,
				Constants.SEED_TRACKER_FILE_NAME,
				setup.FileFormat);

			var trackerPath =
				setup.SaveLocation               +
				Constants.BACKSLASH              +
				Constants.SEED_TRACKER_FILE_NAME +
				Constants.TXT_FILE_TYPE;

			File.AppendAllText(trackerPath, sanitizedName + Environment.NewLine);

			GenLogging.Instance.LogWithTimeStamp(
				LogLevel.Normal,
				stopwatch.TimeElapsed,
				Message.UPDATED_SEED_TRACKER,
				Constants.SERIALIZE_SEED_CTX);
		}
	}
}