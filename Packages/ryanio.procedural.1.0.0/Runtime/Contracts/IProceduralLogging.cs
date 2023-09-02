// ProceduralGeneration

namespace ProceduralGeneration {
	public interface IProceduralLogging {
		void Log(string msg, string ctx);
		void LogWarning(string msg, string ctx);
		void LogError(string msg, string ctx);
		void LogTest(string msg, string ctx);
		void LogCritical(string msg, string ctx);
	}
}