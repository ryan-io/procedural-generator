using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public readonly struct MapCharacteristics {
		public MapCharacteristics(List<List<int>> outlines, List<Vector3> boundaryPositions) {
			Outlines          = outlines;
			BoundaryPositions = boundaryPositions;
		}

		public List<List<int>> Outlines          { get; }
		public List<Vector3>   BoundaryPositions { get; }
	}
}