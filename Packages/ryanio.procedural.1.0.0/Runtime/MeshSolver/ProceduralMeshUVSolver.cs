using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using UnityEngine;

namespace ProceduralGeneration {
	internal class ProceduralMeshUVSolver {
		internal static Vector2[] CalculateUVs(
			ref int[,] map, IReadOnlyList<Vector3> vertices, float squareSize, int tilingMod = 1) {
			var uvs = new Vector2[vertices.Count];

			var rows = map.GetLength(0);
			var cols = map.GetLength(1);
			
			for (var i = 0; i < vertices.Count; i++) {
				var uvX = Mathf.InverseLerp(
					          -rows / 2f * squareSize,
					          rows  / 2f * squareSize,
					          vertices[i].x) * tilingMod;

				var uvY = Mathf.InverseLerp(
					          -cols / 2f * squareSize,
					          cols  / 2f * squareSize,
					          vertices[i].y) * tilingMod;

				uvs[i] = new Vector2(uvX, uvY);
			}

			return uvs;
		}
	}
}