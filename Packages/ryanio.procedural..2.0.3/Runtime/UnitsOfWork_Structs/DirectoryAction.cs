using System;
using System.IO;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityBCL.Serialization;

namespace Engine.Procedural.Runtime {
	public class DirectoryAction {
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

			var directories     = GetMapDirectories(serializableMapName);
			var directoryExists = Directory.Exists(directories.full);

			if (!directoryExists) {
				throw new DirectoryNotFoundException($"Directory not found ({serializableMapName})");
			}

			return directories.full + GetIOFileName(serializableMapName);
		}

		/// <summary>
		///   Returns the directory of the map.
		/// </summary>
		/// <param name="serializableMapName">Map serializable name</param>
		/// <returns>Full path to map directory</returns>
		/// <exception cref="SerializationException">Throws if serializableMapName is not serialized</exception>
		/// <exception cref="DirectoryNotFoundException">Throws if cannot find map directory</exception>
		public string GetMapDirectoryRaw(string serializableMapName) {
			if (string.IsNullOrWhiteSpace(serializableMapName)) {
				return string.Empty;
			}

			var isSerialized = new ValidationSerializedName().Validate(serializableMapName);

			if (!isSerialized)
				throw new SerializationException($"{serializableMapName} {Message.NAME_NOT_SERIALIZED}");

			var directories     = GetMapDirectories(serializableMapName);
			var directoryExists = Directory.Exists(directories.full);

			if (!directoryExists) {
				throw new DirectoryNotFoundException($"Directory not found ({serializableMapName})");
			}

			return directories.raw + GetIOFileName(serializableMapName);
		}

		/// <summary>
		///  Creates a new map directory and returns the full path to the directory.
		/// </summary>
		/// <param name="serializableMapName">Map serializable name</param>
		/// <returns>Full map to directory of map</returns>
		/// <exception cref="SerializationException">Throws if serializableMapName is not serialized</exception>
		[CanBeNull]
		public string CreateNewDirectory(string serializableMapName) {
			try {
				var isSerialized = new ValidationSerializedName().Validate(serializableMapName);

				if (!isSerialized)
					throw new SerializationException($"{serializableMapName} {Message.NAME_NOT_SERIALIZED}");

				var directories     = GetMapDirectories(serializableMapName);
				new DirectoryHelp().ValidateAndCreate(directories.full);

				return directories.full;
			}
			catch (Exception) {
				// ignored
			}

			return string.Empty;
		}

		/// <summary>
		///  Helper method to get the directory of the map. This is intended to be invoked after GetMapDirectory or
		///  GetMapDirectoryRaw is invoked.
		/// </summary>
		/// <param name="serializedName"></param>
		/// <returns>Tuple containing the full directory & raw directory paths</returns>
		public static (string raw, string full) GetMapDirectories(string serializedName) {
			var location     = UnitySaveLocation.GetDefault;
			var internalName = Constants.BACKSLASH      + serializedName + Constants.BACKSLASH;
			var directory    = location.SaveLocation    + internalName;
			var rawDirectory = location.SaveLocationRaw + internalName;
			return new(rawDirectory, directory);
		}

		/// <summary>
		///  Returns a new string based on serializableName that can be used to save/load
		///  files with appended backslashes.
		/// </summary>
		/// <param name="serializableName">Name to add backslashes to</param>	
		/// <returns>string with backslashes added as prefix and suffix</returns>
		string GetIOFileName(string serializableName)
			=> Constants.BACKSLASH + serializableName + Constants.BACKSLASH;
	}
}