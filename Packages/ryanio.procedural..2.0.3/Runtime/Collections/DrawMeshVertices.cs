// Engine.Procedural

using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public class DrawMeshVertices : MonoBehaviour {
		[SerializeField] MeshFilter _meshFilter;

		bool IsSet { get; set; }

		[Button]
		void Set() => IsSet = true;

		void OnDrawGizmosSelected() {
			// if (!IsSet || !_meshFilter)
			// 	return;
			//
			// var vertices = _meshFilter.mesh.vertices;
			//
			// for (var i = 0; i < vertices.Length; i++) {
			// 	DebugExt.DrawCircle(vertices[i], Color.white, true, .5f);
			// }
		}
	}
}