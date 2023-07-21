using System.Collections.Generic;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public class ProceduralMeshUVSolver {
		public Vector2[] CalculateUVs(
			int[,] mapBorder, IReadOnlyList<Vector3> vertices, float squareSize, int tilingMod = 1) {
			var uvs = new Vector2[vertices.Count];

			for (var i = 0; i < vertices.Count; i++) {
				var uvX = Mathf.InverseLerp(
					          -mapBorder.GetLength(0) / 2f * squareSize,
					          mapBorder.GetLength(0)  / 2f * squareSize,
					          vertices[i].x) * tilingMod;

				var uvY = Mathf.InverseLerp(
					          -mapBorder.GetLength(0) / 2f * squareSize,
					          mapBorder.GetLength(0)  / 2f * squareSize,
					          vertices[i].y) * tilingMod;

				uvs[i] = new Vector2(uvX, uvY);
			}

			return uvs;
		}
	}
}