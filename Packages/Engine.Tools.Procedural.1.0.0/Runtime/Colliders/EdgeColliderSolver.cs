using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BCL;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Engine.Procedural {
	public class EdgeCollisionSolver : CollisionSolver {
		public EdgeCollider2D[] Colliders          { get; private set; }
		StopWatchWrapper        StopWatch          { get; }
		Vector2                 EdgeColliderOffset { get; }
		float                   EdgeColliderRadius { get; }
		int                     BorderSize         { get; }
		int                     NumOfRows          { get; }
		int                     NumOfCols          { get; }

		protected override Tilemap BoundaryTilemap { get; }

		/// <summary>
		///					*****   THIS IS AN UNSAFE METHOD   *****
		/// </summary>
		/// <param name="dto">Relevant data transfer object to create colliders</param>
		public override void CreateCollider(CollisionSolverDto dto, [CallerMemberName] string caller = "") {
			try {
				var data = dto.MapData;
				GenLogging.Instance.LogWithTimeStamp(
					LogLevel.Normal,
					StopWatch.TimeElapsed,
					GetVerifyRoomsMsg(data.RoomOutlines.Count),
					CTX);

				var outlineCounter = 0;
				GenLogging.Instance.Log("Total roomoulines: "+ data.RoomOutlines.Count.ToString(), "RoomOUtlines", LogLevel.Test);

				var edgePoints = new List<Vector2>();
				
				foreach (var outline in data.RoomOutlines) {
					if (outline.Count < 2)
						continue;

					edgePoints.Clear();
					outlineCounter++;
					
					GenLogging.Instance.Log(
						"Total nodes for outline #" + outlineCounter + ": " + outline.Count,
						"CreateColliders");

					// iteratorSpan.Clear();
					// outlineSpan.CopyTo(iteratorSpan);

					var roomObject = CreateRoom(dto, outlineCounter);
					var edgeCollider = roomObject.AddComponent<EdgeCollider2D>();
					edgeCollider.offset = EdgeColliderOffset;

					//var workingSlice = iteratorSpan.Slice(0, outlineSpan.Length);

					for (var i = 0; i < outline.Count; i++) {
						var pos = new Vector3(
							data.MeshVertices[outline[i]].x,
							data.MeshVertices[outline[i]].y,
							0);

						if (i >= 1) {
							var lastPost = edgePoints.Last();

							if (Vector2.Distance(pos, lastPost) <= 20f) {
								edgePoints.Add(pos);
							}
						}
						else {
							edgePoints.Add(pos);
							
						}
						
						// var worldPos = BoundaryTilemap.WorldToCell(testPos);
						// var hasTile  = BoundaryTilemap.HasTile(worldPos);

						/*
						if (hasTile) {
							var comparer = new Vector2Int(worldPos.x, worldPos.y);
							var item = hashSet.FirstOrDefault(record =>
								                                  record.Coordinate == comparer &&
								                                  record.IsLocalBoundary);
	
							if (item == null)
								continue;
							
							var shiftedBorder = BorderSize / 2f;
							var shiftedX      = Mathf.CeilToInt(-NumOfRows  / 2f);
							var shiftedY      = Mathf.FloorToInt(-NumOfCols / 2f);
							
							var pos = new Vector2(
								item.Coordinate.x + shiftedX + shiftedBorder,
								item.Coordinate.y + shiftedY + shiftedBorder);
							
							if (i > 0 && i < iteratorLength) {
								if (edgePoints.Count < 1) 
									edgePoints.Add(pos);
	
								else {
									var previousPoint = edgePoints.Last();
	
									if (Vector2.Distance(previousPoint, pos) <= 20.0f)
										edgePoints.Add(pos);
								}
							}
							else 
								if (!edgePoints.Contains(item.Coordinate))
									edgePoints.Add(pos);
						}*/
					}

					if (edgePoints.IsEmptyOrNull() || edgePoints.Count <= 2) {
						if (roomObject) {
#if UNITY_EDITOR

							Object.DestroyImmediate(roomObject);
							
#else
							Object.DestroyImmediate(roomObject);
#endif
							continue;
						}
					}

					var array = edgePoints.ToArray();
					GenLogging.Instance.Log("Total edgepoints: " + edgePoints.Count, "EgePoints");
					edgeCollider.points = array;

					// Colliders[outlineCounter] = edgeCollider;
					// outlineCounter++;
				}
			}

			catch (Exception) {
				GenLogging.Instance.Log(
					"Error thrown from " + caller,
					"EdgeColliderSolver", LogLevel.Error);
			}
		}

		GameObject CreateRoom(CollisionSolverDto dto, int outlineCounter) {
			var roomObject = AddRoom(dto.ColliderGameObject, identifier: outlineCounter.ToString());
			roomObject.MakeStatic(true);
			roomObject.SetLayer(Constants.Layers.OBSTACLES);
			return roomObject;
		}

		int DetermineHighestCollectionCount(MapData data) {
			int currentHighestCount = 0;

			foreach (var outline in data.RoomOutlines) {
				if (outline.Count > currentHighestCount)
					currentHighestCount = outline.Count;
			}

			return currentHighestCount;
		}

		string GetVerifyRoomsMsg(int count) {
			_sB.Clear();
			_sB.Append(VERIFY_ROOMS_PREFIX);
			_sB.Append(count);

			return _sB.ToString();
		}

		string GetCouldNotSetWarning(string roomName, LayerMask layer) {
			_sB.Clear();
			_sB.Append(COULD_NOT_SET_PREFIX);
			_sB.Append(roomName);
			_sB.Append(TO_LAYER);
			_sB.Append(layer.ToString());

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

		const string VERIFY_ROOMS_PREFIX    = "Veriying the number of rooms: ";
		const string COULD_NOT_SET_PREFIX   = "Could not set ";
		const string TO_LAYER               = " to layer ";
		const string CTX                    = "EdgeColliderSolver";
		const int    CLOSED_COLLIDER_POINTS = 4;

		// TODO - Why was 50 chosen?
		const int COLLIDER_ALLOCATION_SIZE = 150;
	}
}