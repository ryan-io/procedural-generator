using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural.Poisson_Spawning {
	public static class PoissonPointGenerator {
		public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize,
			int numSamplesBeforeRejection = 30) {
			var cellSize = radius / Mathf.Sqrt(2);

			var grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize),
				Mathf.CeilToInt(sampleRegionSize.y                / cellSize)];
			var points      = new List<Vector2>();
			var spawnPoints = new List<Vector2>();

			spawnPoints.Add(sampleRegionSize / 2);
			while (spawnPoints.Count > 0) {
				var spawnIndex        = Random.Range(0, spawnPoints.Count);
				var spawnCentre       = spawnPoints[spawnIndex];
				var candidateAccepted = false;

				for (var i = 0; i < numSamplesBeforeRejection; i++) {
					var angle     = Random.value * Mathf.PI * 2;
					var dir       = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
					var candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);
					if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid)) {
						points.Add(candidate);
						spawnPoints.Add(candidate);
						grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
						candidateAccepted                                                  = true;
						break;
					}
				}

				if (!candidateAccepted) spawnPoints.RemoveAt(spawnIndex);
			}

			return points;
		}

		static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius,
			List<Vector2> points, int[,] grid) {
			if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 &&
			    candidate.y < sampleRegionSize.y) {
				var cellX        = (int)(candidate.x / cellSize);
				var cellY        = (int)(candidate.y / cellSize);
				var searchStartX = Mathf.Max(0, cellX - 2);
				var searchEndX   = Mathf.Min(cellX    + 2, grid.GetLength(0) - 1);
				var searchStartY = Mathf.Max(0, cellY - 2);
				var searchEndY   = Mathf.Min(cellY    + 2, grid.GetLength(1) - 1);

				for (var x = searchStartX; x <= searchEndX; x++) {
					for (var y = searchStartY; y <= searchEndY; y++) {
						var pointIndex = grid[x, y] - 1;
						if (pointIndex != -1) {
							var sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
							if (sqrDst < radius * radius) return false;
						}
					}
				}

				return true;
			}

			return false;
		}
	}
}