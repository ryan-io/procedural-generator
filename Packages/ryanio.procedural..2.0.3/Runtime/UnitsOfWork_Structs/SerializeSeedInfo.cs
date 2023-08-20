// Engine.Procedural

using System;
using System.IO;
using BCL.Serialization;
using JetBrains.Annotations;
using UnityBCL;
using UnityBCL.Serialization;
using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct SerializeSeedInfo {
		public void Serialize(SeedInfo seedInfo, string name, [CanBeNull] IProceduralLogging logger) {
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

			File.AppendAllText(path, content + Environment.NewLine);

			logger?.Log(Message.UPDATED_SEED_TRACKER, Constants.SERIALIZE_SEED_CTX);
		}
	}
}