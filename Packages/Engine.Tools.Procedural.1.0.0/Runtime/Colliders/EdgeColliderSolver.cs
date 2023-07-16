using System.Text;
using BCL;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public class EdgeCollisionSolver : CollisionSolver {
		public EdgeCollider2D[] Colliders          { get; }
		StopWatchWrapper        StopWatch          { get; }
		Vector2                 EdgeColliderOffset { get; }
		float                   EdgeColliderRadius { get; }

		public override void CreateCollider(CollisionSolverDto dto) {
			dto.ColliderGameObject.ZeroPosition();
			dto.ColliderGameObject.MakeStatic(false);

			GenLogging.LogWithTimeStamp(
				LogLevel.Normal,
				StopWatch.TimeElapsed,
				GetVerifyRoomsMsg(dto.Outlines.Count),
				CTX);

			for (var i = 0; i < dto.Outlines.Count; i++) {
				var roomObject = AddRoom(dto.ColliderGameObject);
				roomObject.MakeStatic(true);

				if (IsValidLayer(dto.ObstacleLayer))
					roomObject.SetLayer(dto.ObstacleLayer);
				else
					GenLogging.LogWithTimeStamp(
						LogLevel.Warning,
						StopWatch.TimeElapsed,
						GetCouldNotSetWarning(roomObject.name, dto.ObstacleLayer),
						CTX);

				var outline      = dto.Outlines[i];
				var edgeCollider = roomObject.AddComponent<EdgeCollider2D>();
				var edgePoints   = new Vector2[outline.Count];
				var count        = outline.Count;
				edgeCollider.offset = EdgeColliderOffset;

				for (var j = 0; j < count; j++)
					edgePoints[j] = new Vector2(dto.WalkableVertices[outline[j]].x,
						dto.WalkableVertices[outline[j]].y);

				edgeCollider.points = edgePoints;

				if (i >= Colliders.Length)
					continue;

				Colliders[i] = edgeCollider;
			}

			SetColliderRadius(dto);
		}

		string GetVerifyRoomsMsg(int count) {
			_sB.Clear();
			_sB.Append(VERIFY_ROOMS_PREFIX);
			_sB.Append(count);

			return _sB.ToString();
		}

		string GetCouldNotSetWarning(string roomName, LayerMask layer) {
			_sB.Clear();
			_sB.Append(COULD_NOT_SET_PREFIX);
			_sB.Append(roomName);
			_sB.Append(TO_LAYER);
			_sB.Append(layer.ToString());

			return _sB.ToString();
		}

		void SetColliderRadius(CollisionSolverDto dto) {
			var count = dto.Outlines.Count;

			for (var i = 0; i < count; i++) {
				if (i >= Colliders.Length)
					continue;
				Colliders[i].edgeRadius = EdgeColliderRadius;
			}
		}

		bool IsValidLayer(int layerMask) => layerMask is >= 0 and <= 31;

		public EdgeCollisionSolver(ProceduralConfig config, StopWatchWrapper stopWatch) {
			_sB                = new StringBuilder();
			Colliders          = new EdgeCollider2D[COLLIDER_ALLOCATION_SIZE];
			StopWatch          = stopWatch;
			EdgeColliderOffset = config.EdgeColliderOffset;
			EdgeColliderRadius = config.EdgeColliderRadius;
		}

		readonly StringBuilder _sB;
		const    string        VERIFY_ROOMS_PREFIX  = "Veriying the number of rooms: ";
		const    string        COULD_NOT_SET_PREFIX = "Could not set ";
		const    string        TO_LAYER             = " to layer ";
		const    string        CTX                  = "EdgeColliderSolver";

		// TODO - Why was 50 chosen?
		const int COLLIDER_ALLOCATION_SIZE = 50;
	}
}