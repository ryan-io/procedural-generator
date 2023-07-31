using System.Collections.Generic;
using BCL;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public interface IPoisson {
		IEnumerable<GameObject> SpawnObjects(
			List<Vector2> points, WeightedRandom<GameObject> objects, Transform spawnLocation);

		WeightedRandom<GameObject> GetWeightedRandom(GameObjectWeightTable table);
	}
}