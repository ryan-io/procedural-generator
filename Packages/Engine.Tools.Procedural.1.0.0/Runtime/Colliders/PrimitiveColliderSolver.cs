using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProceduralAuxiliary;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Engine.Procedural.Runtime {
	public class PrimitiveCollisionSolver : CollisionSolver {
		protected override Tilemap BoundaryTilemap { get; }
		Vector3                    Char1           { get; set; }
		Vector3                    Char2           { get; set; }
		float                      Slope1          { get; set; }
		float                      Slope2          { get; set; }
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
		/// <param name="caller"></param>
		public override Dictionary<int, List<Vector3>>  CreateCollider(CollisionSolverDto dto, [CallerMemberName] string caller = "") {
			var data     = dto.MapData;
			var dict     = new Dictionary<int, List<Vector3>>();
			
			dto.ColliderGameObject.MakeStatic(true);
			dto.ColliderGameObject.ZeroPosition();

			for (var i = 0; i < data.RoomOutlines.Count; i++) {
				var outLineList = new List<Vector3>();
				dict.Add(i, outLineList);
				
				var col         = CreateNewPrimitiveCollider(dto.ColliderGameObject, i.ToString());
				var outline     = data.RoomOutlines[i];
				//var extractedCorners = col.corners;
				var objList = new GameObject[3];

				for (var k = 0; k < 3; k++) {
					var newPoint = new Vector3(
						data.MeshVertices[outline[k]].x, data.MeshVertices[outline[k]].y, 0);
					col.corners[k].transform.position = newPoint;
					col.corners[k].gameObject.MakeStatic(true);
					objList[k] = col.corners[k].gameObject;
					
					if (!outLineList.Contains(newPoint))
						outLineList.Add(newPoint);
				}

				for (var j = 0; j < outline.Count; j++) {
					var newPoint = new Vector3(data.MeshVertices[outline[j]].x, data.MeshVertices[outline[j]].y, 0);
					CreateHandle(col, newPoint, col.corners[^1], col.corners[^1].GetSiblingIndex() + 1);

					//outLineList.Add(newPoint);
					if (j == 0) {
						Char1 = newPoint;
						
						if (!outLineList.Contains(newPoint))
							outLineList.Add(newPoint);
					}
					else if (j == 1) {
						Char2 = newPoint;
						if (!outLineList.Contains(newPoint))
							outLineList.Add(newPoint);
					}
					else {
						Slope1 = VectorF.GetSlope(Char1, Char2);
						Slope2 = VectorF.GetSlope(Char2, newPoint);
						var hasSlopedChanged = Slope2 - Slope1 == 0;
					
						if (hasSlopedChanged) {
							if (outLineList.Contains(Char2)) 
								outLineList.Remove(Char2);
						}
						else {
							if (!outLineList.Contains(Char2))
								outLineList.Add(Char2);
						}
					}
					
					Char1  = Char2;
					Char2  = newPoint;
				}

				foreach (var obj in objList) {
					if (Application.isEditor)
						Object.DestroyImmediate(obj);
					else
						Object.DestroyImmediate(obj);
				}
			}

			CoreExtensions.SetLayerRecursive(dto.ColliderGameObject, LayerMask.NameToLayer("Boundary"));
			return dict;
		}


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