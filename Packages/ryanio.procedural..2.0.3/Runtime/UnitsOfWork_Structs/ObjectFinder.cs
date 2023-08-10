// Engine.Procedural

using System.Collections.Generic;
using JetBrains.Annotations;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct ObjectFinder {
		public List<GraphColliderCutter> FindGraphColliderCuttersInScene(List<GraphColliderCutter> colliderCutters) {
			if (colliderCutters == null)
				colliderCutters = new List<GraphColliderCutter>();

			var sceneColliderCutters = Object.FindObjectsOfType<GraphColliderCutter>();

			foreach (var cutter in sceneColliderCutters) {
				if (cutter && !colliderCutters.Contains(cutter))
					colliderCutters.Add(cutter);
			}

			return colliderCutters;
		}

		public GameObject FindPathfinderInScene([CanBeNull] GameObject pathfinder) {
			if (pathfinder == null) {
				var pathfinders = Object.FindObjectsOfType<AstarPath>();

				if (pathfinders.IsEmptyOrNull())
					return CreateAndAssignNewPathfinder(pathfinder);
				if (pathfinders.Length > 1)
					return TrimExcessPathfinders(pathfinders, pathfinder);
			}

			return pathfinder;
		}

		GameObject TrimExcessPathfinders(IReadOnlyList<AstarPath> pathfinders, GameObject pathfinder) {
			var firstPathfinder = pathfinders[0];

			for (var i = 0; i < pathfinders.Count; i++) {
				if (i == 0) continue;

				var obj = pathfinders[i];
				if (obj) {
#if UNITY_EDITOR
					Object.DestroyImmediate(obj.gameObject);
#else
					Object.Destroy(obj.gameObject);
#endif
				}
			}

			pathfinder      = firstPathfinder.gameObject;
			pathfinder.name = Constants.PATHFINDING_TAG;
			pathfinder.tag  = Constants.PATHFINDING_TAG;

			return pathfinder;
		}

		GameObject CreateAndAssignNewPathfinder(GameObject pathfinder) {
			pathfinder = new GameObject(Constants.PATHFINDING_TAG) {
				transform = {
					position = Vector3.zero
				},
				tag = Constants.PATHFINDING_TAG
			};

			pathfinder.AddComponent<AstarPath>();

			return pathfinder;
		}
	}
}