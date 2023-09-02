// ProceduralGeneration

using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal class ColliderSolverCtx {
		internal GameObject         Owner              { get; }
		internal ColliderSolverType ColliderSolverType { get; }
		internal TileMapDictionary  TileMapDictionary  { get; }
		internal GameObject         ColliderGo         { get; }
		internal List<List<int>>    RoomOutlines       { get; }
		internal LayerMask          ObstacleLayerMask  { get; }
		internal List<Vector3>      MeshVertices       { get; }
		internal Vector2            EdgeColliderOffset { get; }
		internal Dimensions         Dimensions         { get; }
		internal float              EdgeColliderRadius { get; }
		internal float              SkinWidth          { get; }
		internal int                BorderSize         { get; }

		public ColliderSolverCtx(
			GameObject owner,
			ColliderSolverType colliderSolverType,
			TileMapDictionary tileMapDictionary,
			GameObject colliderGo,
			List<Vector3> meshVertices,
			List<List<int>> roomOutlines,
			LayerMask obstacleLayerMask,
			float skinWidth, Vector2 edgeColliderOffset, float edgeColliderRadius, Dimensions dimensions, int borderSize) {
			Owner              = owner;
			ColliderSolverType = colliderSolverType;
			ColliderGo         = colliderGo;
			SkinWidth          = skinWidth;
			EdgeColliderOffset = edgeColliderOffset;
			EdgeColliderRadius = edgeColliderRadius;
			Dimensions         = dimensions;
			BorderSize    = borderSize;
			TileMapDictionary  = tileMapDictionary;
			RoomOutlines       = roomOutlines;
			MeshVertices       = meshVertices;
			ObstacleLayerMask  = obstacleLayerMask;
		}
	}
}