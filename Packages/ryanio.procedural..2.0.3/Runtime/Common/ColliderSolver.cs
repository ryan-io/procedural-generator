// Engine.Procedural

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BCL;
using UnityEngine;

namespace ProceduralGeneration {
	internal class ColliderSolver {
		ProceduralConfig   Config       { get; }
		StopWatchWrapper   StopWatch    { get; }
		GameObject         ProcGenObj   { get; }
		GameObject         ColliderObj  { get; }
		ColliderSolverType SolverType   { get; }
		LayerMask          ObstacleMask { get; }
		LayerMask          BoundaryMask { get; }

		internal (Dictionary<int, List<Vector3>> SpriteBoundaryCoords, Dictionary<int, List<Vector3>> ColliderCoords) 
			Solve(MapData mapData, TileMapDictionary dictionary, [CallerMemberName] string caller = "") {
			try {
				CollisionSolver solver;

				var dto = new CollisionSolverDto(
					mapData,
					ColliderObj,
					ObstacleMask,
					BoundaryMask);

				if (SolverType == ColliderSolverType.Box)
					solver = new BoxCollisionSolver(Config, dictionary, ProcGenObj);

				else if (SolverType == ColliderSolverType.Edge)
					solver = new EdgeCollisionSolver(Config, dictionary, StopWatch);

				else
					solver = new PrimitiveCollisionSolver(Config, dictionary, ColliderObj);
				
				return (solver.CreateCollider(dto), solver.AllBoundaryPoints);
			}
			catch (Exception) {
				GenLogging.Instance.Log("Error thrown " + caller, "ColliderSolver", LogLevel.Error);
				throw;
			}
		}

		internal ColliderSolver(
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