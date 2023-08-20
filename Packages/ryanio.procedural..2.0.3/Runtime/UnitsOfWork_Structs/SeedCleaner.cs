// ProceduralGeneration

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BCL;
using JetBrains.Annotations;
using UnityBCL;

namespace ProceduralGeneration {
	public static class SeedCleaner {
		/// <summary>
		///  Deletes the seed string for the given serializedName from seedTracker.txt.
		/// </summary>
		/// <param name="serializedName">Name of the map you want to delete</param>
		public static void Delete(string serializedName, [CanBeNull] IProceduralLogging logger) {
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
				logger?.LogWarning(Message.CANNOT_DELETE_SEED + e.Message, nameof(Delete));
			}
		}
	}
}