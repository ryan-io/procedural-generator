using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal class ProceduralMeshUVSolver {
		internal static Vector2[] CalculateUVs(
			int[,] map, IReadOnlyList<Vector3> vertices, float squareSize, int tilingMod = 1) {
			var uvs = new Vector2[vertices.Count];

			for (var i = 0; i < vertices.Count; i++) {
				var uvX = Mathf.InverseLerp(
					          -map.GetLength(0) / 2f * squareSize,
					          map.GetLength(0)  / 2f * squareSize,
					          vertices[i].x) * tilingMod;

				var uvY = Mathf.InverseLerp(
					          -map.GetLength(0) / 2f * squareSize,
					          map.GetLength(0)  / 2f * squareSize,
					          vertices[i].y) * tilingMod;

				uvs[i] = new Vector2(uvX, uvY);
			}

			return uvs;
		}
	}
}