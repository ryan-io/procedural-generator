// Engine.Procedural

using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct MeshCleaner {
		public void Clean(GameObject procGenGo) {
			var meshFilter = procGenGo.GetComponent<MeshFilter>();

			if (meshFilter) {
				meshFilter.mesh = null;
			}
		}
	}
}