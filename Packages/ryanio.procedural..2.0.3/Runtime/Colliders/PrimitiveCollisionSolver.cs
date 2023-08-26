using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProceduralAuxiliary;
using UnityBCL;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	internal class PrimitiveCollisionSolver : CollisionSolver {
		GameObject                     ColliderGo        { get; }
		List<Vector3>                  MeshVertices      { get; }
		List<List<int>>                RoomOutlines      { get; }
		Vector3                        Char1             { get; set; }
		Vector3                        Char2             { get; set; }
		float                          SkinWidth         { get; }
		float                          LastSlope         { get; set; }
		Dictionary<int, List<Vector3>> UnprocessedCoords { get; }
		Dictionary<int, List<Vector3>> ProcessedCoords   { get; }

		/// <summary>
		/// </summary>
		/// <param name="caller"></param>
		internal override Coordinates CreateCollider([CallerMemberName] string caller = "") {
			ColliderGo.MakeStatic(true);
			ColliderGo.ZeroPosition();

			for (var outlineIndex = 0; outlineIndex < RoomOutlines.Count; outlineIndex++) {
				UnprocessedCoords.Add(outlineIndex, new List<Vector3>());
				ProcessCoords(outlineIndex);
			}

			CoreExtensions.SetLayerRecursive(ColliderGo, LayerMask.NameToLayer(Constants.Layer.BOUNDARY));

			return new Coordinates(ProcessedCoords, UnprocessedCoords);
		}

		void ProcessCoords(int currentOutlineIndex) {
			var currentOutline = RoomOutlines[currentOutlineIndex];

			if (currentOutline.Count <= 10)
				return;

			var primitiveCollider = CreateNewPrimitiveCollider(currentOutlineIndex.ToString());

			// PrimitiveCollider API requires a "starting" point of three game objects with colliders
			// this section of the method satisfies this requirement and are later destroyed
			var currentCoordinateList = UnprocessedCoords[currentOutlineIndex];
			var tempObjectList        = SetStarting(currentCoordinateList, currentOutline, primitiveCollider);

			for (var i = 0; i < currentOutline.Count; i++) {
				var newPoint = GetNewPoint(currentOutline, i);

				if (!currentCoordinateList.Contains(newPoint))
					currentCoordinateList.Add(newPoint);
			}

			var tempCopy = new List<Vector3>(currentCoordinateList);

			for (var i = 2; i < currentCoordinateList.Count; i++) {
				var point1 = currentCoordinateList[i - 2];
				var point2 = currentCoordinateList[i - 1];
				var point3 = currentCoordinateList[i];

				if (!VectorF.IsColinear(point1, point2, point3))
					continue;

				if (tempCopy.Contains(point2))
					tempCopy.Remove(point2);
			}

			foreach (var point in tempCopy) {
				CreateHandle(primitiveCollider, point, primitiveCollider.corners[^1], stdIndex);
				stdIndex++;
			}

			foreach (var obj in tempObjectList) {
				if (Application.isEditor)
					Object.DestroyImmediate(obj);
				else
					Object.DestroyImmediate(obj);
			}
		}

		IEnumerable<GameObject> SetStarting(
			ICollection<Vector3> currentCoordinateList,
			IReadOnlyList<int> outline,
			ProceduralPrimitiveCollider col) {
			var tempObjectList = new GameObject[3];

			for (var index = 0; index < 3; index++) {
				var newPoint = GetNewPoint(outline, index);
				tempObjectList[index] = SetupStartingCollider(newPoint, index, col);

				if (!currentCoordinateList.Contains(newPoint))
					currentCoordinateList.Add(newPoint);
			}

			return tempObjectList;
		}

		GameObject SetupStartingCollider(Vector3 point, int k, ProceduralPrimitiveCollider col) {
			col.corners[k].transform.position = point;
			col.corners[k].gameObject.MakeStatic(true);
			return col.corners[k].gameObject;
		}

		int stdIndex      = 0;
		int lastSlopeSign = 0;

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
			col.loop             = true;
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
			SkinWidth         = ctx.SkinWidth;
			ColliderGo        = ctx.ColliderGo;
			RoomOutlines      = ctx.RoomOutlines;
			MeshVertices      = ctx.MeshVertices;
			LastSlope         = Mathf.Infinity;
			UnprocessedCoords = new Dictionary<int, List<Vector3>>();
			ProcessedCoords   = new Dictionary<int, List<Vector3>>();
		}
	}
}