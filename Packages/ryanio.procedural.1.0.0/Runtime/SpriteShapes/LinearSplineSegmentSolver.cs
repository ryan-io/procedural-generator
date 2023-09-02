using BCL;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	public class LinearSplineSegmentSolver : ISplineSegmentSolver {
		PositionSolver  PositionSolver         { get; set; }
		DirectionSolver DirectionSolver        { get; set; }
		bool            ShouldSimplifySegments { get; set; }

		public bool DetermineNextSegment(Spline spline, Vector3 pointPosition, int indexTracker) {
			spline.InsertPointAt(indexTracker, pointPosition);
			spline.SetTangentMode(indexTracker, TANGENT_MODE);

			if (indexTracker < 1) {
				// VERY likely to be south, southwest, or southeast
				// need to think about this more
				DirectionSolver.ForceSetInitialCurrentDirection(CardinalDirection.SouthEast);
				// determine the next segment logic here
				//solver.DetermineNextSegment(spline, indexTracker);

				return true;
			}

			var (next, last) = PositionSolver
			   .GetComparisonPositions(spline, indexTracker, indexTracker - 1);

			if (ShouldSimplifySegments)
				DirectionSolver.AutoDetermineDirection(next, last);

			if (DirectionSolver.CurrentDirection == DirectionSolver.LastDirection) {
				spline.RemovePointAt(indexTracker);
				return false;
			}

			return true;
		}

		public LinearSplineSegmentSolver(SpriteShapeConfig config) {
			DirectionSolver        = new DirectionSolver();
			PositionSolver         = new PositionSolver();
			ShouldSimplifySegments = config.ShouldSimplifySegments;
		}

		const ShapeTangentMode TANGENT_MODE = ShapeTangentMode.Linear;
	}
}