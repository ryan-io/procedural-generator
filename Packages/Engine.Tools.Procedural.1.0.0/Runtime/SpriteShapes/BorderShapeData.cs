// Engine.Procedural

using System;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public class BorderShapeData {
		[field: SerializeField] public SerializedBoundaryShape Shape { get; private set; }

		[field: SerializeField] public SerializedBoundaryPositions ShapePositions { get; private set; }

		[field: SerializeField] public SerializedBoundaryPositions SplinePositions { get; private set; }

		public BorderShapeData(SerializedBoundaryShape shape,
			SerializedBoundaryPositions shapePositions,
			SerializedBoundaryPositions splinePositions) {
			Shape           = shape;
			ShapePositions  = shapePositions;
			SplinePositions = splinePositions;
		}
	}
}