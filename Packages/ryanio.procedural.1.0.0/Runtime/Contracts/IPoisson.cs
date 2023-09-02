using System.Collections.Generic;
using BCL;
using UnityEngine;

namespace ProceduralGeneration {
	public interface IPoisson {
		IEnumerable<GameObject> SpawnObjects(
			List<Vector2> points, WeightedRandom<GameObject> objects, Transform spawnLocation);

		WeightedRandom<GameObject> GetWeightedRandom(GameObjectWeightTable table);
	}
}