// Engine.Procedural

using System;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public class BorderShapeData {
		[field: SerializeField] public SerializedBoundaryPositions SplinePositions { get; private set; }

		public BorderShapeData(SerializedBoundaryPositions splinePositions) {
			SplinePositions = splinePositions;
		}
	}
}