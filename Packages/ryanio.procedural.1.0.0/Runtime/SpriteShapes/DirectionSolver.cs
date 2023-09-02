// Engine.Procedural

using BCL;
using UnityEngine;

namespace ProceduralGeneration {
	public class DirectionSolver {
		public CardinalDirection CurrentDirection { get; set; }
		public CardinalDirection LastDirection    { get; set; }

		public void ForceSetInitialCurrentDirection(CardinalDirection direction) {
			CurrentDirection = direction;
		}

		public void AutoDetermineDirection(Vector3 nextPos, Vector3 lastPos) {
			if (IsMovingEast(nextPos, lastPos)) return;
			if (IsMovingNorthEast(nextPos, lastPos)) return;
			if (IsMovingNorth(nextPos, lastPos)) return;
			if (IsMovingNorthWest(nextPos, lastPos)) return;
			if (IsMovingWest(nextPos, lastPos)) return;
			if (IsMovingSouthWest(nextPos, lastPos)) return;
			if (IsMovingSouth(nextPos, lastPos)) return;
			IsMovingSouthEast(nextPos, lastPos);
		}

		public bool IsMovingEast(Vector3 nextPos, Vector3 lastPos) {
			var isMovingEast = nextPos.x > lastPos.x && Mathf.Abs(nextPos.y - lastPos.y) < Mathf.Epsilon;

			if (isMovingEast && LastDirection == CardinalDirection.East) {
				LastDirection    = CurrentDirection;
				CurrentDirection = CardinalDirection.East;
			}

			return isMovingEast;
		}

		public bool IsMovingNorthEast(Vector3 nextPos, Vector3 lastPos) {
			var isMovingNorthEast = nextPos.x > lastPos.x && nextPos.y > lastPos.y;

			if (isMovingNorthEast) {
				LastDirection    = CurrentDirection;
				CurrentDirection = CardinalDirection.NorthEast;
			}

			return isMovingNorthEast;
		}

		public bool IsMovingNorth(Vector3 nextPos, Vector3 lastPos) {
			var isMovingNorth = Mathf.Abs(nextPos.x - lastPos.x) < Mathf.Epsilon && nextPos.y >= lastPos.y;

			if (isMovingNorth) {
				LastDirection    = CurrentDirection;
				CurrentDirection = CardinalDirection.North;
			}

			return isMovingNorth;
		}

		public bool IsMovingNorthWest(Vector3 next, Vector3 last) {
			var isMovingNorthWest = next.x < last.x && next.y > last.y;

			if (isMovingNorthWest) {
				LastDirection    = CurrentDirection;
				CurrentDirection = CardinalDirection.NorthWest;
			}

			return isMovingNorthWest;
		}

		public bool IsMovingWest(Vector3 next, Vector3 last) {
			var isMovingWest = next.x < last.x && Mathf.Abs(next.y - last.y) < Mathf.Epsilon;

			if (isMovingWest) {
				LastDirection    = CurrentDirection;
				CurrentDirection = CardinalDirection.West;
			}

			return isMovingWest;
		}

		public bool IsMovingSouthWest(Vector3 next, Vector3 last) {
			var isMovingSouthWest = next.x < last.x && next.y < last.y;

			if (isMovingSouthWest) {
				LastDirection    = CurrentDirection;
				CurrentDirection = CardinalDirection.SouthWest;
			}

			return isMovingSouthWest;
		}

		public bool IsMovingSouth(Vector3 next, Vector3 last) {
			var isMovingSouth = Mathf.Abs(next.x - last.x) < Mathf.Epsilon && next.y < last.y;

			if (isMovingSouth) {
				LastDirection    = CurrentDirection;
				CurrentDirection = CardinalDirection.South;
			}

			return isMovingSouth;
		}

		public bool IsMovingSouthEast(Vector3 next, Vector3 last) {
			var isMovingSouthEast = next.x > last.x && next.y < last.y;

			if (isMovingSouthEast) {
				LastDirection    = CurrentDirection;
				CurrentDirection = CardinalDirection.SouthEast;
			}

			return isMovingSouthEast;
		}
	}
}