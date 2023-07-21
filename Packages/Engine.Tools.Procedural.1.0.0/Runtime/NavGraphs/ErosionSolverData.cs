// Engine.Procedural

using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural {
	public readonly struct ErosionSolverData {
		public List<Vector3> NodePositions        { get; }
		public List<Vector3> TilePositions        { get; }
		public List<Vector3> TilePositionsShifted { get; }

		public ErosionSolverData(
			List<Vector3> nodePositions,
			List<Vector3> tilePositions,
			List<Vector3> tilePositionsShifted) {
			NodePositions        = nodePositions;
			TilePositions        = tilePositions;
			TilePositionsShifted = tilePositionsShifted;
		}
	}
}