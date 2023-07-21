using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCL;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Engine.Procedural {
	public class EdgeCollisionSolver : CollisionSolver {
		public EdgeCollider2D[] Colliders          { get; }
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
		public override unsafe void CreateCollider(CollisionSolverDto dto) {
			var data    = dto.MapData;
			var hashSet = data.TileHashset;
			
			// dto.ColliderGameObject.ZeroPosition();
			// dto.ColliderGameObject.MakeStatic(false);

			GenLogging.Instance.LogWithTimeStamp(
				LogLevel.Normal,
				StopWatch.TimeElapsed,
				GetVerifyRoomsMsg(data.RoomOutlines.Count),
				CTX);

			var  highestCount  = DetermineHighestCollectionCount(data);
			int* tempAllocator = stackalloc int[highestCount];
			var  iteratorSpan  = new Span<int>(tempAllocator, highestCount);

			for (var i = 0; i < data.RoomOutlines.Count; i++) {
				iteratorSpan.Clear();
				var outlineSpan = data.RoomOutlines[i].ToArray().AsSpan();
				outlineSpan.CopyTo(iteratorSpan);
				var iteratorLength = outlineSpan.Length;
				var workingSlice   = iteratorSpan[..iteratorLength];

				var roomObject = AddRoom(dto.ColliderGameObject, identifier: i.ToString());
				roomObject.MakeStatic(true);
				roomObject.SetLayer(Constants.Layers.OBSTACLES);

				var edgeCollider = roomObject.AddComponent<EdgeCollider2D>();
				var edgePoints   = new List<Vector2>();

				edgeCollider.offset = EdgeColliderOffset;

				for (var j = 0; j < iteratorLength; j++) {
					var testPos = new Vector3(
						data.MeshVertices[workingSlice[j]].x,
						data.MeshVertices[workingSlice[j]].y,
						0);

					var worldPos = BoundaryTilemap.WorldToCell(testPos);
					var hasTile  = BoundaryTilemap.HasTile(worldPos);

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
					}
				}

				if (edgePoints.IsEmptyOrNull() || edgePoints.Count <= CLOSED_COLLIDER_POINTS) {
#if UNITY_EDITOR
					Object.DestroyImmediate(roomObject);
#else
					Object.DestroyImmediate(roomObject);
#endif
					continue;
				}

				edgeCollider.points = edgePoints.ToArray();

				if (i >= Colliders.Length)
					continue;

				Colliders[i] = edgeCollider;
			}
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
			BoundaryTilemap      = config.TileMapDictionary[TileMapType.Boundary];
			StopWatch          = stopWatch;
			EdgeColliderOffset = config.EdgeColliderOffset;
			EdgeColliderRadius = config.EdgeColliderRadius;
			BorderSize         = config.BorderSize;
			NumOfRows          = config.Rows;
			NumOfCols          = config.Columns;
		}

		readonly StringBuilder _sB;
		
		const    string        VERIFY_ROOMS_PREFIX    = "Veriying the number of rooms: ";
		const    string        COULD_NOT_SET_PREFIX   = "Could not set ";
		const    string        TO_LAYER               = " to layer ";
		const    string        CTX                    = "EdgeColliderSolver";
		const    int           CLOSED_COLLIDER_POINTS = 4;

		// TODO - Why was 50 chosen?
		const int COLLIDER_ALLOCATION_SIZE = 50;
	}
}