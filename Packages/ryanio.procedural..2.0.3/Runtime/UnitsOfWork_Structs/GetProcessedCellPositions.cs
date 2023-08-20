// ProceduralGeneration

using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal class GetProcessedCellPositions {
		Dimensions Dimensions { get; }
		int        BorderSize { get; }

		internal Vector3[] Get(TileHashset hashset) {
			var output = new List<Vector3>();

			foreach (var record in hashset) {
				if (!record.IsMapBoundary)
					continue;

				var shiftedBorder = BorderSize / 2f;
				var shiftedX      = Mathf.CeilToInt(-Dimensions.Rows     / 2f);
				var shiftedY      = Mathf.FloorToInt(-Dimensions.Columns / 2f);

				var pos = new Vector3(
					record.Coordinate.x + shiftedX + shiftedBorder,
					record.Coordinate.y + shiftedY + shiftedBorder,
					0);

				output.Add(pos);
			}

			return output.ToArray();
		}

		internal GetProcessedCellPositions(Dimensions dimensions, int borderSize) {
			Dimensions = dimensions;
			BorderSize = borderSize;
		}
	}
}