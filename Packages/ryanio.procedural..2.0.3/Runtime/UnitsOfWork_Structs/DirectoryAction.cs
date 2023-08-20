using System;
using System.IO;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityBCL.Serialization;
using UnityEditor;

namespace ProceduralGeneration {
	internal class DirectoryAction {
		/// <summary>
		///   Returns the directory of the map.
		/// </summary>
		/// <param name="serializableMapName">Map serializable name</param>
		/// <returns>Full path to map directory</returns>
		/// <exception cref="SerializationException">Throws if serializableMapName is not serialized</exception>
		/// <exception cref="DirectoryNotFoundException">Throws if cannot find map directory</exception>
		internal string GetMapDirectory(string serializableMapName) {
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
		internal string GetMapDirectoryRaw(string serializableMapName) {
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
		internal string CreateNewDirectory(string serializableMapName) {
			try {
				var isSerialized = new ValidationSerializedName().Validate(serializableMapName);

				if (!isSerialized)
					throw new SerializationException($"{serializableMapName} {Message.NAME_NOT_SERIALIZED}");

				var directories = GetMapDirectories(serializableMapName);
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
		internal static (string raw, string full) GetMapDirectories(string serializedName) {
			var location     = UnitySaveLocation.GetDefault;
			var internalName = Constants.BACKSLASH      + serializedName + Constants.BACKSLASH;
			var directory    = location.SaveLocation    + internalName;
			var rawDirectory = location.SaveLocationRaw + internalName;
			return new(rawDirectory, directory);
		}

		/// <summary>
		///  Returns the serialized data directories. This is the default location where all serialized data is stored.
		/// </summary>
		/// <returns>Tuple containing the full directory & raw directory paths for SerializedData</returns>
		internal static (string raw, string full) GetSerializedDataDirectories() {
			var location = UnitySaveLocation.GetDefault;
			return (location.SaveLocationRaw, location.SaveLocation);
		}

		/// <summary>
		///  Returns a new string based on serializableName that can be used to save/load
		///  files with appended backslashes.
		/// </summary>
		/// <param name="serializableName">Name to add backslashes to</param>	
		/// <returns>string with backslashes added as prefix and suffix</returns>
		string GetIOFileName(string serializableName)
			=> Constants.BACKSLASH + serializableName + Constants.BACKSLASH;

		/// <summary>
		/// Deletes the given serialized map directory.
		/// </summary>
		/// <param name="serializedName">Name of map to delete</param>
		/// <param name="actions"></param>
		/// <param name="logger"></param>
		internal static void DeleteDirectory(string serializedName, [CanBeNull] IActions actions, 
			[CanBeNull] IProceduralLogging logger) {
			var directories     = GetMapDirectories(serializedName);
			var directoryExists = Directory.Exists(directories.full);

			if (directoryExists) {
				MetaData.Delete(serializedName, logger);
				Directory.Delete(directories.full, true);
				SeedCleaner.Delete(serializedName, logger);
				AssetDatabase.Refresh();

				actions?.Log($"Deleted {serializedName} directory", nameof(DeleteDirectory));
			}
			else
				actions?.LogError(Message.COULD_NOT_FIND_DIR + serializedName, nameof(DeleteDirectory));
		}
	}
}