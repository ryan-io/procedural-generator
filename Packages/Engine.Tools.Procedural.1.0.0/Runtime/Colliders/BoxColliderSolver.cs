using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Engine.Procedural.Runtime {
	public class BoxCollisionSolver : CollisionSolver {
		GameObject                     RootGameObject { get; }
		Dictionary<string, GameObject> ColliderOwners { get; }
		float                          SkinWidth      { get; }


		protected override Tilemap BoundaryTilemap { get; }

		public override void CreateCollider(CollisionSolverDto dto, List<Vector3> cache, 
			[CallerMemberName] string caller = "") {
			var data = dto.MapData;
			
			CreateRotateColliderObject(ZERO_ANGLE,            0f);
			CreateRotateColliderObject(FORTY_FIVE_ANGLE,      45f);
			CreateRotateColliderObject(ONE_THIRTY_FIVE_ANGLE, 135f);

			var roomObject = AddRoom(dto.ColliderGameObject);
			roomObject.MakeStatic(true);

			for (var i = 0; i < data.RoomOutlines.Count; i++) {
				var outline = data.RoomOutlines[i];
				for (var j = 0; j < outline.Count; j++) {
					if (j + 1 >= outline.Count)
						continue;

					var point0 = data.MeshVertices[outline[j]];
					var point1 =  data.MeshVertices[outline[j + 1]];

					var cx = (point0.x + point1.x) / 2f;
					var cy = (point0.y + point1.y) / 2f;
					var cz = (point0.z + point1.z) / 2f;

					var center = new Vector3(cx, cy, cz);

					var distance = Vector3.Distance(point0, point1);
					var col      = roomObject.AddComponent<BoxCollider>();

					col.center = center;
					col.size   = new Vector3(distance, SkinWidth, 0f);
					cache.Add(center);
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
			BoundaryTilemap  = config.TileMapDictionary[TileMapType.Ground];
		}

		const string ZERO_ANGLE            = "zeroAngle";
		const string FORTY_FIVE_ANGLE      = "fortyFiveAngle";
		const string ONE_THIRTY_FIVE_ANGLE = "oneThirtyFiveAngle";
	}
}