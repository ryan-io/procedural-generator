// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal class SerializedPrimitiveCollisionSolverCtx {
		internal GameObject ColliderGo         { get; }
		internal float      SkinWidth          { get; }
		internal float      Radius { get; }

		public SerializedPrimitiveCollisionSolverCtx(GameObject colliderGo, float skinWidth, float radius) {
			ColliderGo         = colliderGo;
			SkinWidth          = skinWidth;
			Radius = radius;
		}
	}
}