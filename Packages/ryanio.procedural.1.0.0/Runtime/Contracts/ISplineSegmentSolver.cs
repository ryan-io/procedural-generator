// Engine.Procedural

using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	public interface ISplineSegmentSolver {
		bool DetermineNextSegment(Spline spline, Vector3 pointPosition, int indexTracker);
	}
}