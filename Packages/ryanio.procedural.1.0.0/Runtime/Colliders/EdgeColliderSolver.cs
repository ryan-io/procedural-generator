using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BCL;
using Microsoft.Win32.SafeHandles;
using TMPro;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	internal class EdgeCollisionSolver : CollisionSolver {
		public EdgeCollider2D[] Colliders                   { get; private set; }
		Vector2                 LastCalculatedPosition      { get; set; }
		Vector2                 LastCalculatedPoint         { get; set; }
		Vector2                 LastCalculatedStartingPoint { get; set; }
		List<Vector3>           MeshVertices                { get; }
		Vector2                 EdgeColliderOffset          { get; }
		List<List<int>>         RoomOutlines                { get; }
		GameObject              ColliderGo                  { get; }

		float EdgeColliderRadius { get; }
		int   BorderSize         { get; }
		int   NumOfRows          { get; }
		int   NumOfCols          { get; }

		internal override Coordinates CreateCollider([CallerMemberName] string caller = "") {
			ColliderGo.MakeStatic(true);
			ColliderGo.ZeroPosition();

			for (var outlineIndex = 0; outlineIndex < RoomOutlines.Count; outlineIndex++) {
				var currentOutline = RoomOutlines[outlineIndex];
				var points         = Processor.Process(outlineIndex, ref currentOutline);

				ProcessCoords(ref points, outlineIndex);
			}

			ColliderGo.SetLayerRecursive(LayerMask.NameToLayer(Constants.Layer.BOUNDARY));
			
			// foreach (var outline in RoomOutlines) {
			// 	if (outline.Count < 2)
			// 		continue;
			//
			// 	var points     = Processor.Process(outlineCounter);
			// 	outlineCounter++;
			// 	var edgePoints = new List<Vector2>();
			// 	outlineCounter = ProcessOutline(edgePoints, outlineCounter, outline);
			// 	dict.Add(outlineCounter, edgePoints);
			// }

			return new Coordinates(Processor.ProcessedPoints);
		}

		void ProcessCoords(ref List<Vector2> processedPoints, int currentOutlineIndex) {
			var currentOutline = RoomOutlines[currentOutlineIndex];

			if (currentOutline.Count <= 10)
				return;
			
			var roomObject   = CreateRoom(currentOutlineIndex);
			var edgeCollider = roomObject.AddComponent<EdgeCollider2D>();
			edgeCollider.offset = EdgeColliderOffset;
			edgeCollider.points = processedPoints.ToArray();
		}

		int GetLargestCount(List<List<int>> outlines) {
			var output = 0;

			foreach (var item in outlines) {
				if (item.Count > output)
					output = item.Count;
			}

			return output;
		}

		int ProcessOutline(List<Vector2> edgePoints, int outlineCounter, List<int> outline) {
			edgePoints.Clear();
			outlineCounter++;

			var roomObject   = CreateRoom(outlineCounter);
			var edgeCollider = roomObject.AddComponent<EdgeCollider2D>();
			edgeCollider.offset = EdgeColliderOffset;

			var array = outline.ToArray();
			for (var i = 0; i < array.Length; i++)
				DetermineColliderOutline(array, i, edgePoints);

			if (edgePoints.IsEmptyOrNull() || edgePoints.Count <= MIN_POINTS_REQUIRED) {
				if (roomObject) {
#if UNITY_EDITOR

					Object.DestroyImmediate(roomObject);

#else
					Object.DestroyImmediate(roomObject);
#endif
					return outlineCounter;
				}
			}
			else
				edgeCollider.points = edgePoints.ToArray();

			LastCalculatedStartingPoint = edgeCollider.points.First();
			return outlineCounter;
		}

		void DetermineColliderOutline(IReadOnlyList<int> outline, int i, List<Vector2> edgePoints) {
			var pos = new Vector3(
				MeshVertices[outline[i]].x,
				MeshVertices[outline[i]].y,
				0);

			if (i >= 1) {
				var lastPoint = edgePoints.Last();

				if (Vector2.Distance(pos, lastPoint) <= MAX_DISTANCE_BETWEEN_POINTS) {
					// if (!IsInLine(pos.x, lastPoint.x) &&
					//     !IsInLine(pos.y, lastPoint.y)) {
					AddEdgePoint(edgePoints, pos);
					//}
				}
			}
			else {
				edgePoints.Add(pos);
			}

			LastCalculatedPoint = pos;
		}

		void AddEdgePoint(List<Vector2> edgePoints, Vector3 pos) {
			if (!edgePoints.Contains(LastCalculatedPoint))
				edgePoints.Add(LastCalculatedPoint);

			edgePoints.Add(pos);
		}

		static bool IsInLine(float coordComp1, float cordComp2)
			=> Mathf.Abs(coordComp1 - cordComp2) < Constants.FLOATING_POINT_ERROR;

		static bool IsInLine45Angle(Vector2 coord1, Vector2 coord2) {
			var angle = Mathf.Rad2Deg * (Mathf.Atan2(coord2.y - coord1.y, coord2.x - coord1.x));
			angle += 180;

			var is45  = angle is > 44 and < 46;
			var is135 = angle is > 134 and < 136;
			var is225 = angle is > 223 and < 226;
			var is315 = angle is > 314 and < 316;

			return is45 || is135 || is225 || is315;
		}

		GameObject CreateRoom(int outlineCounter) {
			var roomObject = AddRoom(ColliderGo, identifier: outlineCounter.ToString());
			roomObject.MakeStatic(true);
			roomObject.SetLayer(Constants.Layer.OBSTACLES);
			return roomObject;
		}

		internal EdgeCollisionSolver(
			ColliderSolverCtx ctx, 
			ref List<Vector3> meshVertices,
			int cutOffPoints = 10)
			: base(ref meshVertices, cutOffPoints) {
			_sB                = new StringBuilder();
			Colliders          = new EdgeCollider2D[COLLIDER_ALLOCATION_SIZE];
			ColliderGo         = ctx.ColliderGo;
			RoomOutlines       = ctx.RoomOutlines;
			EdgeColliderOffset = ctx.EdgeColliderOffset;
			EdgeColliderRadius = ctx.EdgeColliderRadius;
			BorderSize         = ctx.BorderSize;
			NumOfRows          = ctx.Dimensions.Rows;
			NumOfCols          = ctx.Dimensions.Columns;
			MeshVertices       = ctx.MeshVertices;
		}

		readonly StringBuilder _sB;

		const string VERIFY_ROOMS_PREFIX         = "Veriying the number of rooms: ";
		const string CTX                         = "EdgeColliderSolver";
		const int    MIN_POINTS_REQUIRED         = 3;
		const float  MAX_DISTANCE_BETWEEN_POINTS = 6f;

		// TODO - Why was 50 chosen?
		const int COLLIDER_ALLOCATION_SIZE = 150;
	}
}