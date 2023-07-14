using System.Globalization;
using System.Text;
using BCL;
using UnityBCL;

namespace Engine.Procedural {
	public static class GenLogging {
		// public const string GeneratedMeshBackground = "Generated Mesh Background";
		// public const string GroundLayerName         = "Ground";
		// public const string ObstaclesLayerName      = "Obstacles";
		//
		// public const string NoConfigOrShouldNotGen =
		// 	"Generation at run time has been disabled. Now invoking OnGenerationComplete logic.";
		//
		// public const string LevelGeneratorTag = "LevelGenerator";

		public const string SPACE           = " ";
		const        string TIMESTAMP_LABEL = ":::: TIMESTAMP - ";
		const        string UNIT            = "sec.";

		public static void LogWithTimeStamp(LogLevel level, float totalTime, string msg, string ctx) {
#if UNITY_EDITOR || UNITY_STANDALONE
			Sb.Clear();
			Sb.Append(ctx);
			Sb.Append(TIMESTAMP_LABEL);
			Sb.Append(totalTime.ToString(CultureInfo.InvariantCulture));
			Sb.Append(SPACE);
			Sb.Append(UNIT);

			if (level == LogLevel.Normal)
				Logger.Msg(msg, size: 14, italic: true, ctx: Sb.ToString());
			else if (level == LogLevel.Warning)
				Logger.Warning(msg, size: 14, italic: true, ctx: Sb.ToString());
			else if (level == LogLevel.Error)
				Logger.Error(msg, size: 15, italic: true, bold: true, ctx: Sb.ToString());
#endif
		}

		static readonly UnityLogging  Logger = new();
		static readonly StringBuilder Sb     = new();
	}
}