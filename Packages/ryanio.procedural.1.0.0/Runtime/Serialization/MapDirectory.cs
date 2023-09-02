// ProceduralGeneration

namespace ProceduralGeneration {
	public class PathConstructor {
		string Raw  { get; }
		string Full { get; }

		/// <summary>
		///  Returns the unique path for a map file with the given name and format.
		/// </summary>
		/// <param name="mapName">Serializable map name</param>
		/// <param name="format">File format</param>
		/// <returns></returns>
		public string GetUniquePathRaw(string mapName, string format)
			=> Raw                       +
			   mapName                   +
			   format;

		/// <summary>
		///  Returns the full path for a map file with the given name and format.
		/// </summary>
		/// <param name="mapName">Serializable map name</param>
		/// <param name="format">File format</param>
		/// <returns></returns>
		public string GetUniquePath(string mapName, string format)
			=> Full    +
			   mapName +
			   format;
		
		/// <summary>
		///  Returns the unique path for a map file in JSON format with the given name.
		/// </summary>
		/// <param name="mapName">Serializable map name</param>
		/// <returns></returns>
		public string GetUniquePathRawJson(string mapName) {
			return GetUniquePathRaw(mapName, Constants.JSON_FILE_TYPE);
		}
		
		/// <summary>
		///  Returns the unique path for a map file in prefab format with the given name.
		/// </summary>
		/// <param name="mapName">Serializable map name</param>
		/// <returns></returns>
		public string GetUniquePathRawPrefab(string mapName) {
			return GetUniquePathRaw(mapName, Constants.PREFAB_FILE_TYPE);
		}
		
		/// <summary>
		///  Returns the unique path for a map file in prefab format with the given name. 
		/// </summary>
		/// <param name="mapName">Serializable map name</param>
		/// <returns></returns>
		public string GetUniquePathPrefab(string mapName) {
			return GetUniquePath(mapName, Constants.PREFAB_FILE_TYPE);
		}
		
		/// <summary>
		///  Returns the full path for a map file in JSON format with the given name.
		/// </summary>
		/// <param name="mapName">Serializable map name</param>
		/// <returns></returns>
		public string GetUniquePathJson(string mapName) {
			return GetUniquePath(mapName, Constants.JSON_FILE_TYPE);
		}

		/// <summary>
		///  Constructor for the MapDirectory class.
		/// </summary>
		/// <param name="directories">ValueTuple containing raw and full directory paths</param>
		public PathConstructor((string raw, string full) directories) {
			Raw  = directories.raw;
			Full = directories.full;
		}
	}
}