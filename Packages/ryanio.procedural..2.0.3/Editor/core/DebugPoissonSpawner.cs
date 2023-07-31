using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.Procedural.Editor.Editor {
	[InlineEditor(InlineEditorObjectFieldModes.Boxed)]
	[CreateAssetMenu(fileName = "Poisson Spawner", menuName = "Utility/Debug/Poisson Spawner")]
	public class DebugPoissonSpawner : ScriptableObject {
		[SerializeField] List<Sprite> sprites;
		[SerializeField] int          radius = 1;

		[SerializeField] [MinMaxSlider(10, 500)]
		Vector2Int regionSize = new(15, 15);

		[SerializeField] int samplesBeforeRejection = 30;

		// Add support for points with different radii
		public void Spawn(Vector2 position) {
			try {
				var spawnPoints = GetSpawnPoints(sprites);
			}
			catch (NullReferenceException e) {
				Debug.LogWarning(e.Message);
			}
		}

		IEnumerable<Vector2> GetSpawnPoints(List<Sprite> spawnableSprites) {
			var cellSize    = radius / Mathf.Sqrt(2);
			var gridXPoints = Mathf.CeilToInt(regionSize.x / cellSize);
			var gridYPoints = Mathf.CeilToInt(regionSize.y / cellSize);
			var grid        = new int[gridXPoints, gridYPoints];

			var points              = new List<Vector2>();
			var spawnPoints         = new List<Vector2> { regionSize / 2 };
			var candidPointAccepted = false;

			while (spawnPoints.Count > 0) {
				var index  = Random.Range(0, spawnPoints.Count);
				var center = spawnPoints[index];
				for (var i = 0; i < samplesBeforeRejection; i++) {
					var angle          = Random.value * Mathf.PI * 2;
					var direction      = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
					var candidatePoint = center + direction * Random.Range(radius, 2 * radius);
					if (IsValid(candidatePoint, cellSize, points, grid)) {
						points.Add(candidatePoint);
						spawnPoints.Add(candidatePoint);
						candidPointAccepted                                                          = true;
						grid[GetCellX(candidatePoint, cellSize), GetCellY(candidatePoint, cellSize)] = points.Count;
						break;
					}
				}

				if (!candidPointAccepted) spawnPoints.RemoveAt(index);
			}

			return points;
		}

		bool IsValid(Vector2 point, float cellSize, IReadOnlyList<Vector2> points, int[,] grid) {
			if (!IsInsideRegion(point)) return false;
			var cellX        = GetCellX(point, cellSize);
			var cellY        = GetCellY(point, cellSize);
			var searchStartX = Mathf.Max(0, cellX - 2);
			var searchStartY = Mathf.Max(0, cellY - 2);
			var searchEndX   = Mathf.Max(cellX    + 2, grid.GetLength(0) - 1);
			var searchEndY   = Mathf.Max(cellY    + 2, grid.GetLength(1) - 1);

			for (var i = searchStartX; i < searchEndX; i++) {
				for (var j = searchStartY; j < searchEndY; j++) {
					var pointIndex = grid[i, j] - 1;
					if (pointIndex != -1) {
						var sqDistance = (point - points[pointIndex]).sqrMagnitude;
						if (sqDistance < radius * radius) return false;
					}
				}
			}

			return true;
		}

		bool IsInsideRegion(Vector2 point) =>
			point.x >= 0 && point.x < regionSize.x && point.y >= 0 && point.y < regionSize.y;

		int GetCellX(Vector2 point, float cellSize) => (int)(point.x / cellSize);

		int GetCellY(Vector2 point, float cellSize) => (int)(point.y / cellSize);
	}
}