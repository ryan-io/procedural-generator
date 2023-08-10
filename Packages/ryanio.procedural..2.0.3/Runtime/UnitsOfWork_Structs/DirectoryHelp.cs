// ProceduralGeneration

using System.IO;

namespace ProceduralGeneration {
	public readonly struct DirectoryHelp {
		/// <summary>
		///    Validates the directory path and creates it if it doesn't exist.
		/// </summary>
		/// <returns>True if directory already exists, false if not and creation successful</returns>
		public bool ValidateAndCreate(string directoryPath) {
			var directoryExists = Directory.Exists(directoryPath);

			if (!directoryExists) {
				Directory.CreateDirectory(directoryPath);
				return false;
			}

			return true;
		}
	}
}