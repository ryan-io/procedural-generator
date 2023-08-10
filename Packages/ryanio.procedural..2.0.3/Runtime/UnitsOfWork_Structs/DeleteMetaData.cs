// ProceduralGeneration

using System;
using System.IO;
using BCL;

namespace ProceduralGeneration {
	public static class MetaData {
		/// <summary>
		///  Deletes the meta data file for the given serializedName.
		/// </summary>
		/// <param name="serializedName">Name of the map you want to delete</param>
		public static void Delete(string serializedName) {
			try {
				Help.ValidateNameIsSerialized(serializedName);
				var directories = DirectoryAction.GetSerializedDataDirectories();

				// making an assumption that the serialized data directory exists

				var file = directories.full + Constants.BACKSLASH + serializedName + META;

				if (File.Exists(file))
					File.Delete(file);
			}
			catch (Exception e) {
				GenLogging.Instance.Log(
					$"Could not delete meta data file: {e.Message}",
					"DeleteMetaData",
					LogLevel.Warning);
			}
		}

		const string META = ".meta";
	}
}