// Engine.Procedural

using Pathfinding;

namespace ProceduralGeneration {
	internal abstract class NavGraphBuilder<T> where T : NavGraph {
		internal abstract T Build();
	}
}