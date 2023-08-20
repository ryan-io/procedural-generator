// Engine.Procedural

using System.Collections.Generic;
using System.Linq;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	internal class CreateBoundaryColliders {
		GameObject                ColliderGo { get; }
		TileHashset               Hashset    { get; }
		GetProcessedCellPositions Processor  { get; }
		float                     Radius     { get; }

		internal void Set() {
			var vectors = Processor.Get(Hashset);

			if (vectors.IsEmptyOrNull())
				return;

			var obj = new GameObject("boundary") {
				isStatic  = true,
				transform = { parent = ColliderGo.transform }
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

			var pointList = new List<Vector2> { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };

			edgeCollider.SetPoints(pointList);
			edgeCollider.edgeRadius = Radius;
			new SetAllEdgeColliderRadius(Radius).Set(ColliderGo);
		}

		internal CreateBoundaryColliders(ColliderPointSetterCtx ctx) {
			Processor  = new GetProcessedCellPositions(ctx.Dimensions, ctx.BorderSize);
			ColliderGo = ctx.ColliderGo;
			Hashset    = ctx.Hashset;
			Radius     = ctx.Radius;
		}
	}
}