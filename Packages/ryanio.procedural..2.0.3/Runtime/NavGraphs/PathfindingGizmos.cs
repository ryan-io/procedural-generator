using System.Collections.Generic;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public class PathfindingGizmos {
		public List<Vector3> NodePositions        { get; set; }
		public List<Vector3> TilePositions        { get; set; }
		public List<Vector3> TilePositionsShifted { get; set; }

		bool ShouldDrawNodePositions        { get; }
		bool ShouldDrawTilePositions        { get; }
		bool ShouldDrawTilePositionsShifted { get; }

		public void DrawGizmos() {
			if (!Application.isEditor)
				return;

			if (ShouldDrawNodePositions && !NodePositions.IsEmptyOrNull())
				for (var i = 0; i < NodePositions.Count; i++)
					DebugExt.DrawCircle(NodePositions[i], _normal, Color.cyan, .1f);

			if (ShouldDrawTilePositions && !TilePositions.IsEmptyOrNull())
				for (var i = 0; i < TilePositions.Count; i++)
					DebugExt.DrawCircle(TilePositions[i], _normal, Color.yellow, .1f);

			if (ShouldDrawTilePositionsShifted && !TilePositionsShifted.IsEmptyOrNull())
				for (var i = 0; i < TilePositionsShifted.Count; i++)
					DebugExt.DrawCircle(TilePositionsShifted[i], _normal, Color.white, .1f);

		}

		public PathfindingGizmos(
			bool shouldDrawNodePositions, 
			bool shouldDrawTilePositions,
			bool shouldDrawTilePositionsShifted) {
			ShouldDrawNodePositions        = shouldDrawNodePositions;
			ShouldDrawTilePositions        = shouldDrawTilePositions;
			ShouldDrawTilePositionsShifted = shouldDrawTilePositionsShifted;

			NodePositions        = new List<Vector3>();
			TilePositions        = new List<Vector3>();
			TilePositionsShifted = new List<Vector3>();
		}

		readonly Vector3 _normal = new(0, 0, 1);
	}
}