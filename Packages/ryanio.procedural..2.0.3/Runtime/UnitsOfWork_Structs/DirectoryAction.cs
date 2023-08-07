using System;
using System.IO;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityBCL.Serialization;

namespace Engine.Procedural.Runtime {
	
	public readonly struct DirectoryAction {
		/// <summary>
		///   Returns the directory of the map.
		/// </summary>
		/// <param name="serializableMapName">Map serializable name</param>
		/// <returns>Full path to map directory</returns>
		/// <exception cref="SerializationException">Throws if serializableMapName is not serialized</exception>
		/// <exception cref="DirectoryNotFoundException">Throws if cannot find map directory</exception>
		public string GetMapDirectory(string serializableMapName) {
			if (string.IsNullOrWhiteSpace(serializableMapName)) {
				return string.Empty;
			}

			var isSerialized = new ValidationSerializedName().Validate(serializableMapName);

			if (!isSerialized)
				throw new SerializationException($"{serializableMapName} {Message.NAME_NOT_SERIALIZED}");

			var location        = UnitySaveLocation.GetDefault;
			var directory       = location.SaveLocation + Constants.BACKSLASH + serializableMapName + Constants.BACKSLASH;
			var directoryExists = Directory.Exists(directory);

			if (!directoryExists) {
				throw new DirectoryNotFoundException($"Directory not found ({serializableMapName})");
			}

			return directory;
		}
		
		/// <summary>
		///  Creates a new map directory and returns the full path to the directory.
		/// </summary>
		/// <param name="serializableMapName">Map serializable name</param>
		/// <returns>Full map to directory of map</returns>
		/// <exception cref="SerializationException">Throws if serializableMapName is not serialized</exception>
		[CanBeNull]
		public string NewMapDirectory(string serializableMapName) {
			try {
				var isSerialized = new ValidationSerializedName().Validate(serializableMapName);

				if (!isSerialized)
					throw new SerializationException($"{serializableMapName} {Message.NAME_NOT_SERIALIZED}");

				var location        = UnitySaveLocation.GetDefault;
				var directory       = location.SaveLocation + Constants.BACKSLASH + serializableMapName;
				var directoryExists = Directory.Exists(directory);

				if (!directoryExists)
					Directory.CreateDirectory(directory);

				return directory;
			}
			catch (Exception) {
				// ignored
			}

			return string.Empty;
		}
	}
}