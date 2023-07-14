using System.Collections.Generic;
using BCL;
using UnityEngine;

namespace Engine.Procedural.Poisson_Spawning {
	public interface IPoissonObjectHandler {
		IEnumerable<GameObject> SpawnObjects(
			List<Vector2> points, WeightedRandom<GameObject> objects, Transform spawnLocation);

		WeightedRandom<GameObject> GetWeightedRandom(GameObjectWeightTable table);
	}
}