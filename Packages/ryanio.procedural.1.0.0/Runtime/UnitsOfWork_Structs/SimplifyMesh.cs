using ProceduralAuxiliary.MeshSimplifier;
using UnityEngine;

namespace ProceduralGeneration {
	public readonly struct SimplifyMesh {
		public Mesh Simply(Mesh mesh, float quality = 0.2f) {
			var originalMeshName = mesh.name;
			var meshSimplifier   = new MeshSimplifier();
			meshSimplifier.Initialize(mesh);
			meshSimplifier.SimplifyMesh(quality);
			var simplifiedMesh = meshSimplifier.ToMesh();
			simplifiedMesh.name = $"Simplified-{originalMeshName}";
			return simplifiedMesh;
		}
	}
}