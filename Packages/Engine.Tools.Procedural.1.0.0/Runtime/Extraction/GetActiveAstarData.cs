// Engine.Procedural

using Pathfinding;
using UnityEngine;

namespace Engine.Procedural {
	public readonly struct GetActiveAstarData {
		public AstarData Retrieve() {
#if UNITY_EDITOR
			if (Application.isEditor && !Application.isPlaying)
				AstarPath.FindAstarPath();
#endif
			var data = AstarPath.active.data;
			return data;
		}
	}
}