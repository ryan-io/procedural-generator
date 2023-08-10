// Engine.Procedural

using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	public class PositionSolver {
		public (Vector3 nextPosition, Vector3 lastPosition) GetComparisonPositions(
			Spline spline, int nextIndex, int lastIndex)
			=> (spline.GetPosition(nextIndex), spline.GetPosition(lastIndex));

		public PositionSolver() {
			
		}
	}
}