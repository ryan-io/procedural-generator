// Engine.Procedural

using System.Runtime.CompilerServices;

namespace ProceduralGeneration {
	internal class ColliderSolver {
		internal Coordinates Solve([CallerMemberName] string caller = "") {
			CollisionSolver solver;

			if (_ctx.ColliderSolverType == ColliderSolverType.Box)
				solver = new BoxCollisionSolver(_ctx);

			else if (_ctx.ColliderSolverType == ColliderSolverType.Edge)
				solver = new EdgeCollisionSolver(_ctx);

			else
				solver = new PrimitiveCollisionSolver(_ctx);
			
			return solver.CreateCollider();
		}

		internal ColliderSolver(ColliderSolverCtx ctx) {
			_ctx = ctx;
		}

		readonly ColliderSolverCtx _ctx;
	}
}