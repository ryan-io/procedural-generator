// ProceduralGeneration

using System;
using System.IO;
using BCL;
using JetBrains.Annotations;

namespace ProceduralGeneration {
	public static class MetaData {
		/// <summary>
		///  Deletes the meta data file for the given serializedName.
		/// </summary>
		/// <param name="serializedName">Name of the map you want to delete</param>
		/// <param name="logger">Optional logger</param>
		public static void Delete(string serializedName, [CanBeNull] IProceduralLogging logger) {
			try {
				Help.ValidateNameIsSerialized(serializedName);
				var directories = DirectoryAction.GetSerializedDataDirectories();

				// making an assumption that the serialized data directory exists

				var file = directories.full + Constants.BACKSLASH + serializedName + META;

				if (File.Exists(file))
					File.Delete(file);
			}
			catch (Exception e) {
				logger?.LogWarning(Message.CANNOT_DELETE_META_DATA + e.Message, nameof(Delete));
			}
		}

		const string META = ".meta";
	}
}