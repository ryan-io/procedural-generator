// Engine.Procedural

using System.Collections.Generic;
using System.Linq;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public readonly struct CreateBoundaryColliders {
		DataProcessor Processor { get; }
		float         Radius    { get; }

		public void Create(GameObject parent) {
			var vectors = Processor.GetBorderCellPositions();

			if (vectors.IsEmptyOrNull())
				return;

			var obj = new GameObject("Boundary") {
				isStatic  = true,
				transform = { parent = parent.transform }
			};

			var maxX = vectors.Max(vector => vector.x);
			var minX = vectors.Min(vector => vector.x);
			var maxY = vectors.Max(vector => vector.y); 
			var minY = vectors.Min(vector => vector.y);

			var edgeCollider = obj.AddComponent<EdgeCollider2D>();

			var bottomLeft  = new Vector2(minX, minY);
			var topLeft     = new Vector2(minX, maxY);
			var bottomRight = new Vector2(maxX, minY);
			var topRight    = new Vector2(maxX, maxY);

			var pointList = new List<Vector2>() { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };

			edgeCollider.SetPoints(pointList);
			edgeCollider.edgeRadius = Radius;
		}

		public CreateBoundaryColliders(ProceduralConfig config, DataProcessor processor) {
			Radius    = config.EdgeColliderRadius;
			Processor = processor;
		}
	}
}