using System.Collections.Generic;
using System.Linq;
using BCL;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.Poisson_Spawning {
	[HideMonoScript]
	public class Poisson : Singleton<Poisson, IPoisson>,
	                                    IPoisson {
		public IEnumerable<GameObject> SpawnObjects(
			List<Vector2> points, WeightedRandom<GameObject> objects, Transform spawnLocation) {
			var enumerable = points.ToList();
			var queue      = CreateQueue(enumerable);

			for (var i = 0; i < queue.Count; i++) {
				var spawnPoint  = queue.Dequeue();
				var objToCreate = objects.Pop();
				yield return CreateObject(objToCreate, spawnPoint, spawnLocation);
			}
		}

		public WeightedRandom<GameObject> GetWeightedRandom(GameObjectWeightTable table) {
			if (table == null || table.Count < 1) return default;

			var weightedRandom = new WeightedRandom<GameObject>();
			foreach (var pair in table) weightedRandom.Add(pair.Key, pair.Value);
			return weightedRandom;
		}

		static Queue<Vector2> CreateQueue(IEnumerable<Vector2> points) {
			var pointsList = points.ToList();
			pointsList.FisherYatesShuffle();
			var queue = new Queue<Vector2>(pointsList);
			return queue;
		}

		static GameObject CreateObject(GameObject obj, Vector2 spawnPoint, Transform spawnLocation) {
			var spawnedObj = Instantiate(obj, spawnLocation);
			spawnedObj.transform.localPosition = spawnPoint;
			return spawnedObj;
		}
	}
}