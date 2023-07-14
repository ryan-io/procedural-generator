using Pathfinding;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public static class PathfindingUtil {
		public static void FixPathfinder(ProceduralPathfindingSolverMonobehaviorModel monoModel) {
			if (monoModel == null || monoModel.PathfindingGameObject || Application.isPlaying)
				return;

			if (!monoModel.PathfindingGameObject) {
				var sceneObj = GameObject.FindWithTag("Pathfinding");

				if (sceneObj) monoModel.PathfindingGameObject = sceneObj.FixComponent<AstarPath>().gameObject;
			}

			if (!monoModel.PathfindingGameObject) {
				var obj = Object.FindObjectOfType<AstarPath>();

				if (!obj) {
					var pObj = new GameObject("Pathfinder") {
						transform = {
							position = Vector3.zero
						},
						tag = "Pathfinding"
					};

					var astar = pObj.AddComponent<AstarPath>();
					astar.data.cacheStartup         = true;
					monoModel.PathfindingGameObject = pObj;
				}
			}
		}

		public static byte[] SerializeGraph() => AstarPath.active.data.SerializeGraphs();

		public static void DeserializeGraph(byte[] serializedGraph)
			=> AstarPath.active.data.DeserializeGraphs(serializedGraph);
	}
}