// Engine.Procedural

using UnityEngine;
using UnityEngine.U2D;

namespace Engine.Procedural {
	public interface ISplineSegmentSolver {
		bool DetermineNextSegment(Spline spline, Vector3 pointPosition, int indexTracker);
	}
}