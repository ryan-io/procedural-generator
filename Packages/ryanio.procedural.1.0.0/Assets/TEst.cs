// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.CompilerServices;
// using ProceduralAuxiliary.ProceduralCollider;
// using UnityBCL;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace ProceduralGeneration {
// 	internal class PrimitiveCollisionSolver : CollisionSolver {
// 		public override void Dispose() {
// 			if (IsDisposed)
// 				return;
//
// 			IsDisposed = true;
// 			PointProcessor?.Dispose();
// 		}
// 		
// 		Dictionary<int, List<Vector3>> UnprocessedCoords   { get; }
// 		Dictionary<int, List<Vector3>> ProcessedCoords     { get; }
// 		GameObject                     ColliderGo          { get; }
// 		List<Vector3>                  MeshVertices        { get; }
// 		List<List<int>>                RoomOutlines        { get; }
// 		int                            ProcessedCoordIndex { get; set; }
// 		float                          SkinWidth           { get; }
// 		ColliderPointProcessor         PointProcessor      { get; set; }
// 		
// 		/// <summary>
// 		/// </summary>
// 		/// <param name="caller"></param>
// 		internal override Coordinates CreateCollider([CallerMemberName] string caller = "") {
// 			ColliderGo.MakeStatic(true);
// 			ColliderGo.ZeroPosition();
//
// 			PointProcessor = new ColliderPointProcessor();
//
// 			var v2Verts = new List<Vector2>();
// 			v2Verts.AddRange(MeshVertices.Select(v => new Vector2(v.x, v.y)));
//
// 			var test = PointProcessor.Process(RoomOutlines, v2Verts);
// 			Debug.Log("Creating primitive colliders");
//
// 			for (var outlineIndex = 0; outlineIndex < RoomOutlines.Count; outlineIndex++) {
// 				UnprocessedCoords.Add(outlineIndex, new List<Vector3>());
// 				ProcessedCoords.Add(outlineIndex, new List<Vector3>());
// 				ProcessCoords(outlineIndex);
// 			}
//
// 			CoreExtensions.SetLayerRecursive(ColliderGo, LayerMask.NameToLayer(Constants.Layer.BOUNDARY));
//
// 			return new Coordinates(ProcessedCoords, UnprocessedCoords);
// 		}
//
// 		void ProcessCoords(int currentOutlineIndex) {
// 			var currentOutline = RoomOutlines[currentOutlineIndex];
//
// 			if (currentOutline.Count <= 10)
// 				return;
//
// 			var primitiveCollider = CreateNewPrimitiveCollider(currentOutlineIndex.ToString());
//
// 			var processedOutlineList   = ProcessedCoords[currentOutlineIndex];
// 			var unprocessedOutlineList = UnprocessedCoords[currentOutlineIndex];
//
// 			// PrimitiveCollider API requires a "starting" point of three game objects with colliders
// 			// this section of the method satisfies this requirement and are later destroyed
// 			var tempObjectList = SetStarting(processedOutlineList, currentOutline, primitiveCollider);
//
// 			for (var i = 0; i < currentOutline.Count; i++) {
// 				var newPoint = GetNewPoint(currentOutline, i);
//
// 				if (!processedOutlineList.Contains(newPoint))
// 					processedOutlineList.Add(newPoint);
//
// 				if (!unprocessedOutlineList.Contains(newPoint))
// 					unprocessedOutlineList.Add(newPoint);
// 			}
//
// 			var tempCopy = new List<Vector3>(processedOutlineList);
//
// 			for (var i = 2; i < processedOutlineList.Count; i++) {
// 				DetermineColinearity(processedOutlineList, i, tempCopy);
// 			}
//
// 			foreach (var point in tempCopy) {
// 				CreateHandle(primitiveCollider, point, primitiveCollider.corners[^1], ProcessedCoordIndex);
// 				ProcessedCoordIndex++;
// 			}
//
// 			ProcessedCoords[currentOutlineIndex] = tempCopy;
//
// 			foreach (var obj in tempObjectList) {
// 				if (Application.isEditor)
// 					Object.DestroyImmediate(obj);
// 				else
// 					Object.DestroyImmediate(obj);
// 			}
// 		}
//
// 		static void DetermineColinearity(IReadOnlyList<Vector3> coords, int i, ICollection<Vector3> tempCopy) {
// 			var p1 = coords[i - 2];
// 			var p2 = coords[i - 1];
// 			var p3 = coords[i];
//
// 			if (!VectorF.IsColinear(p1, p2, p3))
// 				return;
//
// 			if (tempCopy.Contains(p2))
// 				tempCopy.Remove(p2);
// 		}
//
// 		IEnumerable<GameObject> SetStarting(
// 			ICollection<Vector3> currentCoordinateList,
// 			IReadOnlyList<int> outline,
// 			ProceduralPrimitiveCollider col) {
// 			var tempObjectList = new GameObject[3];
//
// 			for (var index = 0; index < 3; index++) {
// 				var newPoint = GetNewPoint(outline, index);
// 				tempObjectList[index] = SetupStartingCollider(newPoint, index, col);
//
// 				if (!currentCoordinateList.Contains(newPoint))
// 					currentCoordinateList.Add(newPoint);
// 			}
//
// 			return tempObjectList;
// 		}
//
// 		static GameObject SetupStartingCollider(Vector3 point, int k, ProceduralPrimitiveCollider col) {
// 			col.corners[k].transform.position = point;
// 			col.corners[k].gameObject.MakeStatic(true);
// 			return col.corners[k].gameObject;
// 		}
//
// 		ProceduralPrimitiveCollider CreateNewPrimitiveCollider(string identifier) {
// 			var obj = new GameObject {
// 				name = $"room {identifier} - colliders",
// 				transform = {
// 					position = Vector3.zero,
// 					parent   = ColliderGo.transform
// 				}
// 			};
//
// 			obj.MakeStatic(true);
// 			obj.transform.eulerAngles = new Vector3(90, 0, 0);
//
// 			var col = obj.AddComponent<ProceduralPrimitiveCollider>();
// 			InjectSettings(col);
//
// 			for (var i = 0; i < 3; i++) {
// 				var newObj = new GameObject().transform;
//
// 				newObj.SetParent(col.gameObject.transform);
// 				newObj.localPosition   = Vector3.forward * (0.5f * 5 * i);
// 				newObj.gameObject.name = i.ToString();
//
// 				col.corners.Add(newObj);
// 			}
//
// 			return col;
// 		}
//
// 		void InjectSettings(ProceduralPrimitiveCollider col) {
// 			col.loop             = true;
// 			col.depth            = SkinWidth / 2f;
// 			col.heigth           = SkinWidth / 2f;
// 			col.radius           = SkinWidth;
// 			col.onlyWhenSelected = true;
// 		}
//
// 		static void CreateHandle(Component easyWallCollider, Vector3 newPos, Transform cornerPrototype, int newIndex) {
// 			var newCorner = Object.Instantiate(
// 				cornerPrototype, newPos, Quaternion.identity, easyWallCollider.transform);
//
// 			newCorner.gameObject.MakeStatic(true);
// 			newCorner.SetSiblingIndex(newIndex);
// 		}
//
// 		Vector3 GetNewPoint(IReadOnlyList<int> outline, int index) =>
// 			new(MeshVertices[outline[index]].x, MeshVertices[outline[index]].y, 0);
//
// 		public PrimitiveCollisionSolver(ColliderSolverCtx ctx) {
// 			SkinWidth         = ctx.SkinWidth;
// 			ColliderGo        = ctx.ColliderGo;
// 			RoomOutlines      = ctx.RoomOutlines;
// 			MeshVertices      = ctx.MeshVertices;
// 			UnprocessedCoords = new Dictionary<int, List<Vector3>>();
// 			ProcessedCoords   = new Dictionary<int, List<Vector3>>();
// 		}
// 	}
// }
//
// namespace NAMESPACE {
// 	public class TEst {
// 		
// 	}
// }