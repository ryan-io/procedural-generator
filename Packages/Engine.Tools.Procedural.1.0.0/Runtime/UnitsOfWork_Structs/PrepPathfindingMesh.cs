// Engine.Procedural

using UnityEngine;

namespace Engine.Procedural {
	public readonly struct PrepPathfindingMesh {
		public void Prep(GameObject pathfindingMeshObj, MeshGenerationData meshData) {
			if (pathfindingMeshObj) {
#if UNITY_EDITOR
				Object.DestroyImmediate(pathfindingMeshObj);
#else
				Object.Destroy(pathfindingMeshObj);
#endif
			}

			pathfindingMeshObj = new GameObject {
				name = Constants.PATHFINDING_MESH_LABEL, layer = LayerMask.NameToLayer(Constants.Layers.OBSTACLES)
			};

			var pathfindingMeshFilter = pathfindingMeshObj.AddComponent<MeshFilter>();
			pathfindingMeshFilter.mesh = meshData.GeneratedMesh;

			pathfindingMeshObj.transform.parent = pathfindingMeshObj.transform;
		}
	}
}