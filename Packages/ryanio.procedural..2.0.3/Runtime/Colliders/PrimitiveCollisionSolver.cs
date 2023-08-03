using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProceduralAuxiliary;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Engine.Procedural.Runtime {
	public class PrimitiveCollisionSolver : CollisionSolver {
		protected override Tilemap BoundaryTilemap   { get; }
		GameObject                 ColliderGo        { get; }
		List<Vector3>              MeshVertices      { get; set; }
		Vector3                    Char1             { get; set; }
		Vector3                    Char2             { get; set; }
		bool                       CurrentIsColinear { get; set; }
		bool                       LastWasColinear   { get; set; }
		float                      SkinWidth         { get; }
		float                      Radius            { get; }

		/// <summary>
		/// 3 points (x1,y1), (x2,y2), and (x3,y3) are colinear (in a line) if:
		/// (x2-x1)(y3-y2) - (y2-y1)(x3-x2) = 0
		/// </summary>
		/// <param name="dto"></param>
		/// <param name="caller"></param>
		public override Dictionary<int, List<Vector3>> CreateCollider(CollisionSolverDto dto,
			[CallerMemberName] string caller = "") {
			MeshVertices = dto.MapData.MeshVertices;
			var roomOutlines = dto.MapData.RoomOutlines;
			var dict         = new Dictionary<int, List<Vector3>>();

			ColliderGo.MakeStatic(true);
			ColliderGo.ZeroPosition();

			for (var i = 0; i < roomOutlines.Count; i++) {
				var outlines = new List<Vector3>();
				AllBoundaryPoints.Add(i, new List<Vector3>());
				dict.Add(i, outlines);

				InstantiateCollider(outlines, roomOutlines, i);
			}

			CoreExtensions.SetLayerRecursive(ColliderGo, LayerMask.NameToLayer("Boundary"));
			return dict;
		}

		void InstantiateCollider(ICollection<Vector3> outlines, IReadOnlyList<List<int>> roomOutlines, int index) {
			var col     = CreateNewPrimitiveCollider(index.ToString());
			var outline = roomOutlines[index];
			var objList = new GameObject[3];

			// PrimitiveCollider API requires a "starting" point of three game objects with colliders
			// this section of the method satisfies this requirement and are later destroyed
			SetStarting(outlines, outline, col, objList);

			for (var i = 0; i < outline.Count; i++) {
				var allBoundaryList = AllBoundaryPoints[index];
				var newPoint        = GetNewPoint(outline, i);

				if (!allBoundaryList.Contains(newPoint))
					allBoundaryList.Add(newPoint);

				CreateBodyColliders(newPoint, i, col, outlines);
			}

			foreach (var obj in objList) {
				if (Application.isEditor)
					Object.DestroyImmediate(obj);
				else
					Object.DestroyImmediate(obj);
			}
		}

		void SetStarting(ICollection<Vector3> outlines, List<int> outline, ProceduralPrimitiveCollider col,
			GameObject[] objList) {
			for (var k = 0; k < 3; k++) {
				var newPoint = GetNewPoint(outline, k);
				CreateOriginColliders(newPoint, k, col, objList);

				if (!outlines.Contains(newPoint))
					outlines.Add(newPoint);
			}
		}

		void CreateOriginColliders(Vector3 point, int k, ProceduralPrimitiveCollider col, IList<GameObject> objList) {
			col.corners[k].transform.position = point;
			col.corners[k].gameObject.MakeStatic(true);
			objList[k] = col.corners[k].gameObject;
		}

		void CreateBodyColliders(Vector3 point, int index, ProceduralPrimitiveCollider col,
			ICollection<Vector3> outlines) {
			CreateHandle(col, point, col.corners[^1], col.corners[^1].GetSiblingIndex() + 1);

			if (index == 0)
				ValidateAndAddFirst(outlines, point);
			else if (index == 1)
				ValidateAndAddSecond(outlines, point);
			else
				ValidateAndAddNew(outlines, point);

			Char1           = Char2;
			Char2           = point;
			LastWasColinear = CurrentIsColinear;
		}

		// Vector3 DefineAndCreateHandle(IReadOnlyList<int> outline, int index, ProceduralPrimitiveCollider col) {
		// 	var newPoint = GetNewPoint(outline, index);
		// 	CreateHandle(col, newPoint, col.corners[^1], col.corners[^1].GetSiblingIndex() + 1);
		// 	return newPoint;
		// }

		void ValidateAndAddFirst(ICollection<Vector3> outlines, Vector3 newPoint) {
			Char1 = newPoint;

			if (!outlines.Contains(newPoint))
				outlines.Add(newPoint);
		}

		void ValidateAndAddSecond(ICollection<Vector3> outlines, Vector3 newPoint) {
			Char2 = newPoint;
			if (!outlines.Contains(newPoint))
				outlines.Add(newPoint);
		}

		void ValidateAndAddNew(ICollection<Vector3> outlines, Vector3 newPoint) {
			CurrentIsColinear = VectorF.IsColinear(Char1, Char2, newPoint);

			if (CurrentIsColinear) {
				if (outlines.Contains(Char2))
					outlines.Remove(Char2);
			}
			else {
				if (LastWasColinear) {
					if (!outlines.Contains(Char1)) {
						//var last = outLineList.Last();

						//if (Vector3.Distance(Char1, last) < CullDistance)
						outlines.Add(Char1);
					}
				}

				if (!outlines.Contains(newPoint)) {
					//var last = outLineList.Last();

					//if (Vector3.Distance(newPoint, last) <CullDistance)
					outlines.Add(newPoint);
				}
			}
		}


		ProceduralPrimitiveCollider CreateNewPrimitiveCollider(string identifier) {
			var obj = new GameObject {
				name = $"room {identifier} - colliders",
				transform = {
					position = Vector3.zero,
					parent   = ColliderGo.transform
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

		void CreateHandle(Component easyWallCollider, Vector3 newPos, Transform cornerPrototype, int newIndex) {
			var newCorner = Object.Instantiate(
				cornerPrototype, newPos, Quaternion.identity, easyWallCollider.transform);

			newCorner.gameObject.MakeStatic(true);
			newCorner.SetSiblingIndex(newIndex);
		}

		Vector3 GetNewPoint(IReadOnlyList<int> outline, int index) =>
			new(MeshVertices[outline[index]].x, MeshVertices[outline[index]].y, 0);

		public PrimitiveCollisionSolver(ProceduralConfig config, TileMapDictionary dictionary, GameObject colliderGo) {
			SkinWidth       = config.PrimitiveColliderRadius;
			Radius          = config.PrimitiveColliderRadius;
			BoundaryTilemap = dictionary[TileMapType.Ground];
			ColliderGo      = colliderGo;
		}
	}
}