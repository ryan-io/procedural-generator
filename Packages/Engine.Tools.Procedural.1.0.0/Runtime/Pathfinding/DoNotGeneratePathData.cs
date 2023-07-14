using System.Threading;

namespace Engine.Procedural {
	public readonly struct DoNotGeneratePathData {
		public PathfindingProgressData Data              { get; }
		public CancellationToken       CancellationToken { get; }

		public DoNotGeneratePathData(PathfindingProgressData data, CancellationToken cancellationToken) {
			Data              = data;
			CancellationToken = cancellationToken;
		}
	}
}