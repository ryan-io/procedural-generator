// ProceduralGeneration

namespace ProceduralGeneration {
	internal partial class Actions {
		public void Log(string msg, string ctx)         => _logger.Log(msg, ctx);
		public void LogWarning(string msg, string ctx)  => _logger.LogWarning(msg, ctx);
		public void LogError(string msg, string ctx)    => _logger.LogError(msg, ctx);
		public void LogTest(string msg, string ctx)     => _logger.LogTest(msg, ctx);
		public void LogCritical(string msg, string ctx) => _logger.LogCritical(msg, ctx);
	}
}