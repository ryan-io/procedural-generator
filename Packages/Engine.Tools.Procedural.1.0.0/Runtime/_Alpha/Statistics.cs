using System;
using System.IO;
using System.Text;
using Engine.Tools.Serializer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Engine.Procedural {
	public class ProceduralMapStats : MonoBehaviour {
		// public ProceduralMapStatsSo CreateStatistics() {
		// 	var seed = _model.ProceduralComponents.ProceduralMapSolver.Model.SeedValue.ToString();
		// }
		[SerializeField] [HideLabel] ProceduralMapStatsMonoModel _model;
	}

	[Serializable]
	public class ProceduralMapStatsMonoModel {
		[field: SerializeField]
		[field: Required]
		[field: Title("Required Procedural Components")]
		public FlowComponents ProceduralComponents { get; private set; }
	}

	public class ProceduralMapStatsSo : ScriptableObject {
		[SerializeField] public string Seed;
		[SerializeField] public int    IterationNumber;
	}
	

	public static class ProceduralMapStatsHelper {
		const string SerializationFolder = "ProceduralSeedTracker/";
		const string SeedTrackerName     = "seedTracker";

		public static void WriteNewSeed(MapStats stats, SerializerSetup setup) {
			var sb = new StringBuilder();

			sb.Append(stats.NameOfMap + "_");
			sb.Append($"{stats.Seed}");
			sb.Append("_luid");
			sb.Append($"{stats.Iteration}");

			Serializer.EnsureDirectoryExists(setup.SaveRoot + SerializationFolder, setup.SaveRoot);
			Serializer.EnsureFileExists(setup.SaveRoot      + SerializationFolder, SeedTrackerName, ".txt");

			var trackerPath = setup.SaveRoot   + SerializationFolder + "/" + SeedTrackerName + ".txt";
			File.AppendAllText(trackerPath, sb + Environment.NewLine);
			Debug.Log("Updated the procedural seed tracker.");
		}
	}
}