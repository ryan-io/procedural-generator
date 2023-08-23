using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProceduralAuxiliary;
using UnityBCL;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	internal class PrimitiveCollisionSolver : CollisionSolver {
		GameObject      ColliderGo   { get; }
		List<Vector3>   MeshVertices { get; }
		List<List<int>> RoomOutlines { get; }
		Vector3         Char1        { get; set; }
		float           SkinWidth    { get; }
		float           LastSlope    { get; set; }

		/// <summary>
		/// </summary>
		/// <param name="caller"></param>
		internal override Coordinates CreateCollider([CallerMemberName] string caller = "") {
			var spriteBorderCoords = new Dictionary<int, List<Vector3>>();
			var colliderCoords     = new Dictionary<int, List<Vector3>>();

			ColliderGo.MakeStatic(true);
			ColliderGo.ZeroPosition();

			for (var i = 0; i < RoomOutlines.Count; i++) {
				var spriteBorderVectors = new List<Vector3>();
				colliderCoords.Add(i, new List<Vector3>());
				spriteBorderCoords.Add(i, spriteBorderVectors);

				InstantiateCollider(colliderCoords, spriteBorderVectors, i);
			}

			CoreExtensions.SetLayerRecursive(ColliderGo, LayerMask.NameToLayer(Constants.Layer.BOUNDARY));

			return new Coordinates(spriteBorderCoords, colliderCoords);
		}

		void InstantiateCollider(
			Dictionary<int, List<Vector3>> colliderCoords,
			IList<Vector3> spriteBorderVectors,
			int index) {
			var outline = RoomOutlines[index];

			if (outline.Count <= 10)
				return;

			var col     = CreateNewPrimitiveCollider(index.ToString());
			var objList = new GameObject[3];

			// PrimitiveCollider API requires a "starting" point of three game objects with colliders
			// this section of the method satisfies this requirement and are later destroyed
			SetStarting(spriteBorderVectors, outline, col, objList);

			for (var i = 0; i < outline.Count; i++) {
				var allBoundaryList = colliderCoords[index];
				var newPoint        = GetNewPoint(outline, i);

				if (!allBoundaryList.Contains(newPoint))
					allBoundaryList.Add(newPoint);

				CreateBodyColliders(newPoint, i, col, spriteBorderVectors);
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

		int stdIndex      = 0;
		int lastSlopeSign = 0;

		void CreateBodyColliders(Vector3 point, int index, ProceduralPrimitiveCollider col,
			ICollection<Vector3> spriteBorderVectors) {
			//col.corners[^1].GetSiblingIndex() + 1
			if (index == 0) {
				CreateHandle(col, point, col.corners[^1], stdIndex);
				stdIndex++;
				ValidateAndAddFirst(spriteBorderVectors, point);
				return;
			}

			var slope     = VectorF.GetSlope(point, Char1);
			var slopeSign = Math.Sign(slope);
			var areEqual  = Mathf.Abs(slope - LastSlope) < Constants.FLOATING_POINT_ERROR;

			// if (lastSlopeSign != 0) {
			// 	if (slopeSign != lastSlopeSign) {
			// 		areEqual = false;  
			// 	}
			// }

			if (!areEqual) {
				if (!spriteBorderVectors.Contains(Char1)) {
					CreateHandle(col, Char1, col.corners[^1], stdIndex);
					stdIndex++;
					spriteBorderVectors.Add(Char1);
				}
			}

			LastSlope     = slope;
			lastSlopeSign = slopeSign;
			Char1         = point;
		}

		void ValidateAndAddFirst(ICollection<Vector3> outlines, Vector3 newPoint) {
			Char1 = newPoint;

			if (!outlines.Contains(newPoint))
				outlines.Add(newPoint);
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

				col.corners.Add(newObj);
			}

			return col;
		}

		void InjectSettings(ProceduralPrimitiveCollider col) {
			col.depth            = SkinWidth / 2f;
			col.heigth           = SkinWidth / 2f;
			col.radius           = SkinWidth;
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

		public PrimitiveCollisionSolver(ColliderSolverCtx ctx) {
			SkinWidth    = ctx.SkinWidth;
			ColliderGo   = ctx.ColliderGo;
			RoomOutlines = ctx.RoomOutlines;
			MeshVertices = ctx.MeshVertices;
			LastSlope    = Mathf.Infinity;
		}
	}
}