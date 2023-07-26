// Engine.Procedural

using UnityEngine;
using UnityEngine.U2D;

namespace Engine.Procedural {
	public class ContinuousSplineSegmentSolver : ISplineSegmentSolver {
		public bool DetermineNextSegment(Spline spline, Vector3 pointPosition, int indexTracker) {
			spline.InsertPointAt(indexTracker, pointPosition);
			spline.SetTangentMode(indexTracker, TANGENT_MODE);

			if (indexTracker < 1) {
				_tangentSetter.SetTangentDefaultZeroVector(spline, indexTracker);
				return true;
			}

			var (next, last) = _positionSolver.GetComparisonPositions(spline, indexTracker, indexTracker - 1);

			if (_directionSolver.IsMovingEast(next, last))
				_tangentSetter.SetTangentsQuad14(spline, indexTracker);

			else if (_directionSolver.IsMovingNorthEast(next, last))
				_tangentSetter.SetTangentsQuad1(spline, indexTracker);

			else if (_directionSolver.IsMovingNorth(next, last))
				_tangentSetter.SetTangentsQuad12(spline, indexTracker);

			else if (_directionSolver.IsMovingNorthWest(next, last))
				_tangentSetter.SetTangentsQuad2(spline, indexTracker);

			else if (_directionSolver.IsMovingWest(next, last))
				_tangentSetter.SetTangentsQuad23(spline, indexTracker);

			else if (_directionSolver.IsMovingSouthWest(next, last))
				_tangentSetter.SetTangentsQuad3(spline, indexTracker);

			else if (_directionSolver.IsMovingSouth(next, last))
				_tangentSetter.SetTangentsQuad34(spline, indexTracker);


			else if (_directionSolver.IsMovingSouth(next, last))
				_tangentSetter.SetTangentsQuad34(spline, indexTracker);

			else if (_directionSolver.IsMovingSouthEast(next, last))
				_tangentSetter.SetTangentsQuad4(spline, indexTracker);

			else
				_tangentSetter.SetTangentDefaultUnitVector(spline, indexTracker);

			return true;
		}

		public ContinuousSplineSegmentSolver() {
			_positionSolver  = new PositionSolver();
			_directionSolver = new DirectionSolver();
			_tangentSetter   = new TangentSetter();
		}

		readonly DirectionSolver _directionSolver;
		readonly PositionSolver  _positionSolver;
		readonly TangentSetter   _tangentSetter;

		const ShapeTangentMode TANGENT_MODE = ShapeTangentMode.Continuous;
	}
}