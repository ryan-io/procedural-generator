// Engine.Procedural

namespace Engine.Procedural.Runtime {
	public abstract class MeshSolver {
		public abstract MeshSolverData SolveAndCreate(int[,] mapBorder);
	}
}