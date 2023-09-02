// ProceduralGeneration

using Pathfinding;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct GridGraphBuilderCtx {
		internal Dimensions   Dimensions           { get; }
		internal ColliderType ColliderType         { get; }
		internal LayerMask    ObstacleMask         { get; }
		internal float        GraphCollideDiameter { get; }
		internal float        GraphNodeSize        { get; }

		public GridGraphBuilderCtx(
			Dimensions dimensions,
			ColliderType colliderType,
			LayerMask obstacleMask,
			float graphCollideDiameter,
			float graphNodeSize) {
			Dimensions           = dimensions;
			ColliderType         = colliderType;
			ObstacleMask         = obstacleMask;
			GraphCollideDiameter = graphCollideDiameter;
			GraphNodeSize        = graphNodeSize;
		}
	}
}