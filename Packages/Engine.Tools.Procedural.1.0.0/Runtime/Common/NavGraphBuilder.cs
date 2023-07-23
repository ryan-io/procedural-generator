// Engine.Procedural

using Pathfinding;

namespace Engine.Procedural.Runtime {
	public abstract class NavGraphBuilder<T> where T : NavGraph {
		public abstract T Build();
	}
}