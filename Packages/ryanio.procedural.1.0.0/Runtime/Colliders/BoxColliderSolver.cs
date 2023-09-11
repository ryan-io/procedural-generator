using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	internal class BoxCollisionSolver : CollisionSolver {
		GameObject                     RootGameObject     { get; }
		GameObject                     ColliderGameObject { get; }
		Dictionary<string, GameObject> ColliderOwners     { get; }
		List<List<int>>                RoomOutlines       { get; }
		List<Vector3>                  MeshVertices       { get; }
		Tilemap                        Tilemap            { get; }
		LayerMask                      ObstacleLayerMask  { get; }
		float                          SkinWidth          { get; }

		internal override Coordinates CreateCollider([CallerMemberName] string caller = "") {
			//var data = dto.MapData;
			var dict = new Dictionary<int, List<Vector2>>();

			CreateRotateColliderObject(ZERO_ANGLE,            0f);
			CreateRotateColliderObject(FORTY_FIVE_ANGLE,      45f);
			CreateRotateColliderObject(ONE_THIRTY_FIVE_ANGLE, 135f);

			var roomObject = AddRoom(ColliderGameObject);
			roomObject.MakeStatic(true);

			for (var i = 0; i < RoomOutlines.Count; i++) {
				var outline     = RoomOutlines[i];
				var outlineList = new List<Vector2>();
				dict.Add(i, outlineList);

				for (var j = 0; j < outline.Count; j++) {
					if (j + 1 >= outline.Count)
						continue;

					var point0 = MeshVertices[outline[j]];
					var point1 = MeshVertices[outline[j + 1]];

					var cx = (point0.x + point1.x) / 2f;
					var cy = (point0.y + point1.y) / 2f;
					var cz = (point0.z + point1.z) / 2f;

					var center = new Vector3(cx, cy, cz);

					var distance = Vector3.Distance(point0, point1);
					var col      = roomObject.AddComponent<BoxCollider>();

					col.center = center;
					col.size   = new Vector3(distance, SkinWidth, 0f);
					outlineList.Add(center);
				}
			}

			roomObject.SetLayer(ObstacleLayerMask);
			return new Coordinates(dict);
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

		internal BoxCollisionSolver(ColliderSolverCtx ctx) {
			ColliderOwners     = new Dictionary<string, GameObject>();
			RootGameObject     = ctx.Owner;
			ColliderGameObject = ctx.ColliderGo;
			RoomOutlines       = ctx.RoomOutlines;
			SkinWidth          = ctx.SkinWidth;
			Tilemap            = ctx.TileMapDictionary[TileMapType.Ground];
		}

		const string ZERO_ANGLE            = "zeroAngle";
		const string FORTY_FIVE_ANGLE      = "fortyFiveAngle";
		const string ONE_THIRTY_FIVE_ANGLE = "oneThirtyFiveAngle";
	}
}