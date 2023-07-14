// Engine.Procedural

using Unity.Mathematics;
using UnityEngine;

namespace Engine.Procedural {
	public struct GridSetEvent {
		public int2 MapSize { get; }

		public Grid GridObj { get; }

		public GridSetEvent(Grid gridObj, int2 mapSize) {
			GridObj = gridObj;
			MapSize = mapSize;
		}
	}
}