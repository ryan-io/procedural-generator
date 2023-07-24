using CommunityToolkit.HighPerformance;

namespace Engine.Procedural.Runtime {
	public abstract class SmoothMapSolver {
		public abstract void Smooth(int[,] primaryMap);
	}
}