// ProceduralGeneration

using Pathfinding;
using UnityEngine;

namespace ProceduralGeneration {
	internal interface IColliderConfig {
		ColliderType       GetColliderType();
		ColliderSolverType GetColliderSolverType();
		Vector2            GetEdgeColliderOffset();
		float              GetSkinWidth();
		float              GetEdgeColliderRadius();
	}
}