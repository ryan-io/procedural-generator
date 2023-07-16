using System.Collections.Generic;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public class PathfindingGizmos {
		public List<Vector3> NodePositions        { get; } = new();
		public List<Vector3> TilePositions        { get; } = new();
		public List<Vector3> TilePositionsShifted { get; } = new();
		
		bool ShouldDrawNodePositions        { get; }
		bool ShouldDrawTilePositions        { get; }
		bool ShouldDrawNodePositionsShifted { get; }
		bool ShouldDrawTilePositionsShifted { get; }

		public void DrawGizmos() {
			if (!Application.isEditor)
				return;

			if (ShouldDrawNodePositions)
				for (var i = 0; i < NodePositions.Count; i++)
					DebugExt.DrawCircle(NodePositions[i], _normal, Color.cyan, .1f);

			if (ShouldDrawTilePositions)
				for (var i = 0; i < TilePositions.Count; i++)
					DebugExt.DrawCircle(TilePositions[i], _normal, Color.yellow, .1f);

			if (ShouldDrawTilePositionsShifted)
				for (var i = 0; i < TilePositionsShifted.Count; i++)
					DebugExt.DrawCircle(TilePositionsShifted[i], _normal, Color.white, .1f);

			if (ShouldDrawTilePositionsShifted)
				for (var i = 0; i < TilePositionsShifted.Count; i++)
					DebugExt.DrawCircle(TilePositionsShifted[i], _normal, Color.white, .1f);
		}

		public PathfindingGizmos(bool shouldDrawNodePositions, bool shouldDrawTilePositions, bool shouldDrawNodePositionsShifted, bool shouldDrawTilePositionsShifted) {
			ShouldDrawNodePositions             = shouldDrawNodePositions;
			ShouldDrawTilePositions             = shouldDrawTilePositions;
			ShouldDrawNodePositionsShifted      = shouldDrawNodePositionsShifted;
			ShouldDrawTilePositionsShifted = shouldDrawTilePositionsShifted;
		}

		readonly Vector3 _normal = new(0, 0, 1);
	}
}