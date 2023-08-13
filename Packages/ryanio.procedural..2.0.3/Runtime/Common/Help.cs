using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityBCL;
using UnityBCL.Serialization;

namespace ProceduralGeneration {
	public static class Help {
		/// <summary>
		///  Gets and reads text from seedTracker.txt file.
		///  If the file does not exist, an empty collection is returned.
		///  This is also used in inspectors that need to display all seeds or invoke logic based on the seeds.
		/// </summary>
		/// <returns>Enumerable of type string</returns>
		public static IEnumerable<string> GetAllSeeds() {
			var location = UnitySaveLocation.GetDefault;
			var path     = location.GetFilePath(Constants.SEED_TRACKER_FILE_NAME, Constants.TXT_FILE_TYPE);

			var hasFile = File.Exists(path);

			if (!hasFile)
				return Enumerable.Empty<string>();

			var lines = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

			if (lines.IsEmptyOrNull())
				return Enumerable.Empty<string>();

			var newLines = new string[lines.Length];

			for (var i = 0; i < newLines.Length; i++) {
				var l = lines[i];
				newLines[i] = l.Replace(Constants.SAVE_SEED_PREFIX, "");
			}

			return newLines.AsEnumerable();
		}

		/// <summary>
		///  Gets all seeds and the path to the seedTracker.txt file.
		/// </summary>
		/// <param name="trackerPath">Pre-allocated string to return path of the tracker file</param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllSeedsWithFile(out string trackerPath) {
			var seeds = GetAllSeeds();

			var location = UnitySaveLocation.GetDefault;
			trackerPath = location.GetFilePath(Constants.SEED_TRACKER_FILE_NAME, Constants.TXT_FILE_TYPE);
			return seeds;
		}

		/// <summary>
		///  Helper method for validating that the given name is serialized. This needs to be captured in a try/catch block.
		/// </summary>
		/// <param name="name">Name to serialize</param>
		/// <exception cref="Exception">Throws if name is not serialized</exception>
		public static void ValidateNameIsSerialized(string name) {
			if (string.IsNullOrWhiteSpace(name))
				throw new NullReferenceException("Name was null or empty. Please pass a valid map name.");

			var isSerialized = new ValidationSerializedName().Validate(name);

			if (!isSerialized) {
				throw new Exception(Message.NO_NAME_FOUND + name);
			}
		}

		/// <summary>
		/// Clears the Unity console window.
		/// </summary>
		public static void ClearConsole() {
			var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
			var type     = assembly.GetType("UnityEditor.LogEntries");
			var method   = type.GetMethod("Clear");
			method?.Invoke(new object(), null);
		}
	}
}