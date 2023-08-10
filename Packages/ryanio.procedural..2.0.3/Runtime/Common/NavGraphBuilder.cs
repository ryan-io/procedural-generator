// Engine.Procedural

using Pathfinding;

namespace ProceduralGeneration {
	public abstract class NavGraphBuilder<T> where T : NavGraph {
		public abstract T Build();
	}
}