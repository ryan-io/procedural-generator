using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProceduralAuxiliary;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Engine.Procedural.Runtime {
	public readonly struct PointCharacterization {
		public Vector3 Vector   { get; }
		public bool    IsCorner { get; }

		public PointCharacterization(Vector3 vector, bool isCorner) {
			Vector   = vector;
			IsCorner = isCorner;
		}
	}

	public class PrimitiveCollisionSolver : CollisionSolver {
		protected override Tilemap BoundaryTilemap { get; }
		Vector3                    LastPosition1   { get; set; }
		Vector3                    LastPosition2   { get; set; }
		float                      SkinWidth       { get; }
		float                      Radius          { get; }
		float                      FortyFour       { get; }
		float                      FortySix        { get; }
		float                      OneThirtyFour   { get; }
		float                      OneThirtySix    { get; }

		/// <summary>
		/// 3 points (x1,y1), (x2,y2), and (x3,y3) are colinear (in a line) if:
		/// (x2-x1)(y3-y2) - (y2-y1)(x3-x2) = 0
		/// </summary>
		/// <param name="dto"></param>
		/// <param name="cache"></param>
		/// <param name="caller"></param>
		public override void CreateCollider(CollisionSolverDto dto, List<Vector3> cache,
			[CallerMemberName] string caller = "") {
			var data = dto.MapData;

			dto.ColliderGameObject.MakeStatic(true);
			dto.ColliderGameObject.ZeroPosition();

			for (var i = 0; i < data.RoomOutlines.Count; i++) {
				var col     = CreateNewPrimitiveCollider(dto.ColliderGameObject, i.ToString());
				var outline = data.RoomOutlines[i];
				//var extractedCorners = col.corners;
				var objList = new GameObject[3];

				for (var k = 0; k < 3; k++) {
					var newPoint = new Vector3(
						data.MeshVertices[outline[k]].x, data.MeshVertices[outline[k]].y, 0);
					col.corners[k].transform.position = newPoint;
					col.corners[k].gameObject.MakeStatic(true);
					objList[k] = col.corners[k].gameObject;
				}


				for (var j = 0; j < outline.Count; j++) {
					var newPoint = new Vector3(data.MeshVertices[outline[j]].x, data.MeshVertices[outline[j]].y, 0);

					CreateHandle(col, newPoint, col.corners[^1], col.corners[^1].GetSiblingIndex() + 1);
					if (j == 0) {
						LastPosition1 = newPoint;
						cache.Add(newPoint);
					}
					else if (j == 1) {
						LastPosition2 = newPoint;
						cache.Add(newPoint);
					}
					else {
						//(x2 -x1)(y3 -y2) - (y2 -y1)(x3 -x2) = 0
						// p3 = newpoint; p2 = LastPosition2; p1 = LastPosition1
						var testValue = (LastPosition2.x - LastPosition1.x) * (newPoint.y - LastPosition2.y) -
						                (LastPosition2.y - LastPosition1.y) * (newPoint.x - LastPosition2.x);

						if (testValue < 0.2f && testValue > -0.2f) {
							Debug.Log("LAST 3 POINTS ARE COLINEAR");
							cache.Remove(LastPosition2);
							cache.Add(newPoint);
						}


						// if (Mathf.Abs(newPoint.x - LastPosition1.x) < Constants.FLOATING_POINT_ERROR) {
						// 	Debug.Log("Vectors are in-line horizontally");
						// 	cache.Add(newPoint);
						// }
						// else if (Mathf.Abs(newPoint.y - LastPosition1.y) < Constants.FLOATING_POINT_ERROR) {
						// 	Debug.Log("Vectors are in-line vertically");
						// 	cache.Add(newPoint);
						// }
						// else {
						// 	var newCheckVector  = newPoint     + Vector3.right;
						// 	var lastCheckVector = LastPosition1 + Vector3.right;
						// 	var angle           = Vector2.Angle(newCheckVector, lastCheckVector);
						// 	if ((angle > FortyFour     && angle < FortySix) ||
						// 	    (angle > OneThirtyFour && angle < OneThirtSix)) {
						// 		Debug.Log("POINT SHOULD NOT BE DRAWN HERE " + Mathf.Rad2Deg * angle);
						// 		cache.Add(newPoint);
						// 	}
						// }
					}

					LastPosition1 = LastPosition2;
					LastPosition2 = newPoint;
				}

				foreach (var obj in objList) {
					if (Application.isEditor)
						Object.DestroyImmediate(obj);
					else
						Object.DestroyImmediate(obj);
				}
			}

			CoreExtensions.SetLayerRecursive(dto.ColliderGameObject, LayerMask.NameToLayer("Boundary"));
		}


		bool IsInLine(float coordComp1, float cordComp2)
			=> Mathf.Abs(coordComp1 - cordComp2) < Constants.FLOATING_POINT_ERROR;

		ProceduralPrimitiveCollider CreateNewPrimitiveCollider(GameObject parent, string identifier) {
			var obj = new GameObject {
				name = $"room {identifier} - colliders",
				transform = {
					position = Vector3.zero,
					parent   = parent.transform
				}
			};

			obj.MakeStatic(true);
			obj.transform.eulerAngles = new Vector3(90, 0, 0);

			var col = obj.AddComponent<ProceduralPrimitiveCollider>();
			InjectSettings(col);

			for (var i = 0; i < 3; i++) {
				var newObj = new GameObject().transform;
				newObj.SetParent(col.gameObject.transform);
				newObj.localPosition   = Vector3.forward * (0.5f * 5 * i);
				newObj.gameObject.name = i.ToString();
#if UNITY_EDITOR
				//PolygonColliderEditorExtention.DrawIcon(newObj.gameObject, 0);
#endif
				col.corners.Add(newObj);
			}

			//orcePopulateCorners();
			return col;
		}

		void InjectSettings(ProceduralPrimitiveCollider col) {
			col.depth            = SkinWidth / 2f;
			col.heigth           = SkinWidth / 2f;
			col.radius           = Radius;
			col.onlyWhenSelected = true;
		}

		void CreateHandle(
			Component easyWallCollider, Vector3 newPos, Transform cornerPrototype, int newIndex) {
			var newCorner = Object.Instantiate(
				cornerPrototype, newPos, Quaternion.identity, easyWallCollider.transform);

			newCorner.gameObject.MakeStatic(true);
			newCorner.SetSiblingIndex(newIndex);
		}

		public PrimitiveCollisionSolver(ProceduralConfig config) {
			SkinWidth       = config.PrimitiveColliderRadius;
			Radius          = config.PrimitiveColliderRadius;
			BoundaryTilemap = config.TileMapDictionary[TileMapType.Ground];

			FortyFour     = Mathf.Deg2Rad * 42f;
			FortySix      = Mathf.Deg2Rad * 48f;
			OneThirtyFour = Mathf.Deg2Rad * 132f;
			OneThirtySix  = Mathf.Deg2Rad * 138f;
		}
	}
}