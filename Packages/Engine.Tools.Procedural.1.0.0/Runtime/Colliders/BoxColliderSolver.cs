using System.Collections.Generic;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.ColliderSolver {
	public class BoxCollisionSolver : CollisionSolver {
		GameObject                     RootGameObject { get; }
		Dictionary<string, GameObject> ColliderOwners { get; }
		float                          SkinWidth      { get; }


		public override void CreateCollider(CollisionSolverDto dto) {
			CreateRotateColliderObject(ZERO_ANGLE,            0f);
			CreateRotateColliderObject(FORTY_FIVE_ANGLE,      45f);
			CreateRotateColliderObject(ONE_THIRTY_FIVE_ANGLE, 135f);

			var roomObject = AddRoom(dto.ColliderGameObject);
			roomObject.MakeStatic(true);

			for (var i = 0; i < dto.Outlines.Count; i++) {
				var outline = dto.Outlines[i];
				for (var j = 0; j < outline.Count; j++) {
					if (j + 1 >= outline.Count)
						continue;

					var point0 = dto.WalkableVertices[outline[j]];
					var point1 = dto.WalkableVertices[outline[j + 1]];

					var cx = (point0.x + point1.x) / 2f;
					var cy = (point0.y + point1.y) / 2f;
					var cz = (point0.z + point1.z) / 2f;

					var center = new Vector3(cx, cy, cz);

					var distance = Vector3.Distance(point0, point1);
					var col      = roomObject.AddComponent<BoxCollider>();

					col.center = center;
					col.size   = new Vector3(distance, SkinWidth, 0f);
				}
			}

			roomObject.SetLayer(dto.ObstacleLayer);
		}

		void CreateRotateColliderObject(string id, float angle) {
			var obj = new GameObject {
				name     = id,
				isStatic = true,
				transform = {
					eulerAngles = new Vector3(0, 0, angle),
					parent      = RootGameObject.transform
				}
			};

			ColliderOwners.Add(id, obj);
		}

		public BoxCollisionSolver(ProceduralConfig config, GameObject rootGameObject) {
			ColliderOwners = new Dictionary<string, GameObject>();
			RootGameObject = rootGameObject;
			SkinWidth      = config.BoxColliderSkinWidth;
		}

		const string ZERO_ANGLE            = "zeroAngle";
		const string FORTY_FIVE_ANGLE      = "fortyFiveAngle";
		const string ONE_THIRTY_FIVE_ANGLE = "oneThirtyFiveAngle";
	}
}