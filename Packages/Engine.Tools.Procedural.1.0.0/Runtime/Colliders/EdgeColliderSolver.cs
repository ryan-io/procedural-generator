using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BCL;
using Pathfinding.Util;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Engine.Procedural {
	public class EdgeCollisionSolver : CollisionSolver {
		public EdgeCollider2D[] Colliders           { get; private set; }
		StopWatchWrapper        StopWatch           { get; }
		Vector2                 LastCalculatedPoint { get; set; }
		Vector2                 LastCalculatedStartingPoint { get; set; }
		Vector2                 EdgeColliderOffset  { get; }

		float EdgeColliderRadius { get; }
		int   BorderSize         { get; }
		int   NumOfRows          { get; }
		int   NumOfCols          { get; }

		protected override Tilemap BoundaryTilemap { get; }

		/// <summary>
		///					*****   THIS IS AN UNSAFE METHOD   *****
		/// </summary>
		/// <param name="dto">Relevant data transfer object to create colliders</param>
		public override void CreateCollider(CollisionSolverDto dto, [CallerMemberName] string caller = "") {
			try {
				var data = dto.MapData;
				LogOutlineCount(data);
				var outlineCounter = 0;
				LogRoomOutlineCount(data);

				var edgePoints     = new List<Vector2>();
				var allocationSize = GetLargestCount(data.RoomOutlines);

				// int* pointer = stackalloc int[allocationSize];
				// var  span    = new Span<int>(pointer, allocationSize);

				
				
				foreach (var outline in data.RoomOutlines) {
					// span.Clear();
					// var array = outline.ToArray();
					//
					// for (var i = 0; i < array.Length; i++)
					// 	span[i] = array[i];

					if (outline.Count < 2)
						continue;

					outlineCounter = ProcessOutline(dto, edgePoints, outlineCounter, outline, data);
				}
			}

			catch (Exception) {
				GenLogging.Instance.Log(
					"Error thrown from " + caller,
					"EdgeColliderSolver", LogLevel.Error);
			}
		}

		int GetLargestCount(List<List<int>> outlines) {
			var output = 0;

			foreach (var item in outlines) {
				if (item.Count > output)
					output = item.Count;
			}

			return output;
		}

		static void LogRoomOutlineCount(MapData data) {
			GenLogging.Instance.Log("Total roomoulines: " + data.RoomOutlines.Count.ToString(), "RoomOUtlines",
				LogLevel.Test);
		}

		void LogOutlineCount(MapData data) {
			GenLogging.Instance.LogWithTimeStamp(
				LogLevel.Normal,
				StopWatch.TimeElapsed,
				GetVerifyRoomsMsg(data.RoomOutlines.Count),
				CTX);
		}

		int ProcessOutline(
			CollisionSolverDto dto, List<Vector2> edgePoints,
			int outlineCounter,
			List<int> outline,
			MapData data) {
			edgePoints.Clear();
			outlineCounter++;

			GenLogging.Instance.Log(
				"Total nodes for outline #" + outlineCounter + ": " + outline.Count,
				"CreateColliders");

			var roomObject   = CreateRoom(dto, outlineCounter);
			var edgeCollider = roomObject.AddComponent<EdgeCollider2D>();
			edgeCollider.offset = EdgeColliderOffset;

			var  array   = outline.ToArray();
			// int* pointer = stackalloc int[array.Length];
			// var  span    = new Span<int>(pointer, array.Length);

			// for (var i = 0; i < array.Length; i++) {
			// 	span[i] = array[i];
			// }

			for (var i = 0; i < array.Length; i++)
				DetermineColliderOutline(data, array, i, edgePoints);

			// if (Vector2.Distance(edgePoints.First(), edgePoints.Last()) < 3) {
			// 	edgePoints.Add(edgePoints.First());
			// }
			//
			// if (Vector2.Distance(LastCalculatedStartingPoint, edgePoints.Last()) < 3) {
			// 	edgePoints.Add(LastCalculatedStartingPoint);
			// }

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

		void DetermineColliderOutline(MapData data, IReadOnlyList<int> outline, int i, List<Vector2> edgePoints) {
			var pos = new Vector3(
				data.MeshVertices[outline[i]].x,
				data.MeshVertices[outline[i]].y,
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

		GameObject CreateRoom(CollisionSolverDto dto, int outlineCounter) {
			var roomObject = AddRoom(dto.ColliderGameObject, identifier: outlineCounter.ToString());
			roomObject.MakeStatic(true);
			roomObject.SetLayer(Constants.Layers.OBSTACLES);
			return roomObject;
		}

		string GetVerifyRoomsMsg(int count) {
			_sB.Clear();
			_sB.Append(VERIFY_ROOMS_PREFIX);
			_sB.Append(count);

			return _sB.ToString();
		}

		public EdgeCollisionSolver(ProceduralConfig config, StopWatchWrapper stopWatch) {
			_sB                = new StringBuilder();
			Colliders          = new EdgeCollider2D[COLLIDER_ALLOCATION_SIZE];
			BoundaryTilemap    = config.TileMapDictionary[TileMapType.Boundary];
			StopWatch          = stopWatch;
			EdgeColliderOffset = config.EdgeColliderOffset;
			EdgeColliderRadius = config.EdgeColliderRadius;
			BorderSize         = config.BorderSize;
			NumOfRows          = config.Rows;
			NumOfCols          = config.Columns;
		}

		readonly StringBuilder _sB;

		const string VERIFY_ROOMS_PREFIX         = "Veriying the number of rooms: ";
		const string CTX                         = "EdgeColliderSolver";
		const int    MIN_POINTS_REQUIRED         = 4;
		const float  MAX_DISTANCE_BETWEEN_POINTS = 10f;

		// TODO - Why was 50 chosen?
		const int COLLIDER_ALLOCATION_SIZE = 150;
	}
}