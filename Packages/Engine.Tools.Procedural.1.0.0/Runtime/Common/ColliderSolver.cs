// Engine.Procedural

using BCL;
using UnityEngine;

namespace Engine.Procedural {
	public class ColliderSolver {
		ProceduralConfig   Config       { get; }
		StopWatchWrapper   StopWatch    { get; }
		GameObject         ProcGenObj  { get; }
		GameObject         ColliderObj  { get; }
		ColliderSolverType SolverType   { get; }
		LayerMask          ObstacleMask { get; }
		LayerMask          BoundaryMask { get; }

		public void Solve(MapData mapData) {
			CollisionSolver solver;

			var dto = new CollisionSolverDto(
				mapData,
				ColliderObj,
				ObstacleMask,
				BoundaryMask);

			if (SolverType == ColliderSolverType.Box)
				solver = new BoxCollisionSolver(Config, ProcGenObj);

			else if (SolverType == ColliderSolverType.Edge)
				solver = new EdgeCollisionSolver(Config, StopWatch);

			else
				solver = new PrimitiveCollisionSolver(Config);

			solver.CreateCollider(dto);
		}

		public ColliderSolver(
			ProceduralConfig config, 
			GameObject procGenObj,
			GameObject colliderObj, 
			StopWatchWrapper stopWatch) {
			Config       = config;
			ObstacleMask = config.ObstacleLayerMask;
			BoundaryMask = config.BoundaryLayerMask;
			SolverType   = config.SolverType;
			ProcGenObj   = procGenObj;
			ColliderObj  = colliderObj;
			StopWatch    = stopWatch;
		}
	}
}