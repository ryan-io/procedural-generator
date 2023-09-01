using System.Globalization;
using System.Text;
using BCL;
using UnityBCL;

namespace ProceduralGeneration {
	internal class GenLogging : IProceduralLogging {
		public const string SPACE           = " ";
		const        string TIMESTAMP_LABEL = ":::: ELAPSED - ";
		const        string UNIT            = "mSec.";

		StopWatchWrapper StopWatch { get; }

		void IProceduralLogging.Log(string msg, string ctx) => Log(msg, ctx);

		void IProceduralLogging.LogWarning(string msg, string ctx) => Log(msg, ctx, LogLevel.Warning);

		void IProceduralLogging.LogError(string msg, string ctx) => Log(msg, ctx, LogLevel.Error);

		void IProceduralLogging.LogTest(string msg, string ctx) => LogWithTimeStamp(LogLevel.Normal, msg, ctx);

		void IProceduralLogging.LogCritical(string msg, string ctx) => LogWithTimeStamp(LogLevel.Error, msg, ctx);

		public void Log(string msg, string ctx, LogLevel level = LogLevel.Normal) {
#if UNITY_EDITOR || UNITY_STANDALONE
			Sb.Clear();
			Sb.Append(ctx);

			if (level == LogLevel.Normal)
				Logger.Msg(msg, size: 14, italic: true, ctx: Sb.ToString());
			else if (level == LogLevel.Warning)
				Logger.Warning(msg, size: 14, italic: true, ctx: Sb.ToString());
			else if (level == LogLevel.Error)
				Logger.Error(msg, size: 15, italic: true, bold: true, ctx: Sb.ToString());
#endif
		}

		internal void LogWithTimeStamp(LogLevel level, string msg, string ctx) {
#if UNITY_EDITOR || UNITY_STANDALONE
			Sb.Clear();
			Sb.Append(ctx);
			Sb.Append(TIMESTAMP_LABEL);
			Sb.Append(StopWatch.TimeElapsed);
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

		internal GenLogging(StopWatchWrapper stopWatch) {
			StopWatch = stopWatch;
		}

		static readonly UnityLogging  Logger = new();
		static readonly StringBuilder Sb     = new();
	}
}