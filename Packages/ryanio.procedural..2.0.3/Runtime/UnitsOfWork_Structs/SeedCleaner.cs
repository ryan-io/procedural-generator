// ProceduralGeneration

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BCL;
using UnityBCL;

namespace ProceduralGeneration {
	public static class SeedCleaner {
		/// <summary>
		///  Deletes the seed string for the given serializedName from seedTracker.txt.
		/// </summary>
		/// <param name="serializedName">Name of the map you want to delete</param>
		public static void Delete(string serializedName) {
			try {
				Help.ValidateNameIsSerialized(serializedName);
				var seeds = Help.GetAllSeedsWithFile(out var trackerPath).ToList();

				if (string.IsNullOrWhiteSpace(trackerPath) || seeds.IsEmptyOrNull())
					return;

				if (seeds.Contains(serializedName)) {
					seeds.Remove(serializedName);					
					File.WriteAllLines(trackerPath,  seeds);
				}
			}
			catch (Exception e) {
				GenLogging.Instance.Log(
					"Could not delete seed from tracker file: " + e.Message, "DeleteSeed", LogLevel.Warning);
			}
		}
	}
}