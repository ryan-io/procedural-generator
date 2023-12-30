// Engine.Procedural

using System;
using System.Runtime.CompilerServices;

namespace ProceduralGeneration {
	internal class ColliderSolver : IDisposable {
		protected bool IsDisposed { get; set; }

		CollisionSolver Solver { get; set; }

		internal Coordinates Solve([CallerMemberName] string caller = "") {
			var meshVertices    = _ctx.MeshVertices;
			
			if (_ctx.ColliderSolverType == ColliderSolverType.Edge)
				Solver = new EdgeCollisionSolver(_ctx, ref meshVertices);

			else
				Solver = new PrimitiveCollisionSolver(_ctx, ref meshVertices);

			return Solver.CreateCollider();
		}

		internal ColliderSolver(ColliderSolverCtx ctx) {
			_ctx = ctx;
		}

		readonly ColliderSolverCtx _ctx;

		public virtual void Dispose() {
			if (IsDisposed)
				return;
			
			Solver?.Dispose();
		}
	}
}