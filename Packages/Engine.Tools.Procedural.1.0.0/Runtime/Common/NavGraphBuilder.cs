// Engine.Procedural

using Pathfinding;

namespace Engine.Procedural {
	public abstract class NavGraphBuilder<T> where T : NavGraph {
		public abstract T Build();
	}
}