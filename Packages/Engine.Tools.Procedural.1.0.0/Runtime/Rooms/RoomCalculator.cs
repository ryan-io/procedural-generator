using System;
using System.Collections.Generic;
using System.Linq;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Engine.Procedural {
	public class DataProcessor {
		Tilemap             BoundaryTilemap { get; }
		MapData             MapData         { get; }
		IReadOnlyList<Room> Rooms           { get; }
		Grid                GridToDrawOn    { get; }
		Region              EdgeTiles       { get; set; }
		Vector2             OffSet          { get; }
		int                 NumOfRows       { get; }
		int                 NumOfCols       { get; }
		int                 BorderSize      { get; }

		public void DrawRooms() {
			if (Rooms.IsEmptyOrNull()) {
				return;
			}

			var colorCounter = 0;
			var roomSpan     = Rooms.ToArray().AsSpan();

			foreach (var room in roomSpan) {
				var edgeTileSpan = room.EdgeTiles.ToArray().AsSpan();

				foreach (var edgeTile in edgeTileSpan) {
					var pos = GridToDrawOn.CellToWorld(new Vector3Int(edgeTile.x, edgeTile.y, 0));
					DebugExt.DrawPoint(pos, Constants.Color[colorCounter], 2f);
				}

				colorCounter++;
			}
		}

		/// <summary>
		///					*****   THIS IS AN UNSAFE METHOD   *****
		/// </summary>
		public unsafe void DrawRoomOutlines() {
			if (Rooms.IsEmptyOrNull()) {
				return;
			}

			var colorCounter = 0;
			var outlineSpan  = MapData.RoomOutlines.ToArray().AsSpan();

			var  currentHighestCount = DetermineHighestRoomCount();
			int* tempAllocator       = stackalloc int[currentHighestCount];
			var  iteratorSpan        = new Span<int>(tempAllocator, currentHighestCount);

			foreach (var outline in outlineSpan) {
				var groupSpan = outline.ToArray().AsSpan();

				iteratorSpan.Clear();
				groupSpan.CopyTo(iteratorSpan);

				for (var i = 0; i < groupSpan.Length; i++) {
					var point = new Vector3(
						MapData.MeshVertices[iteratorSpan[i]].x + OffSet.x,
						MapData.MeshVertices[iteratorSpan[i]].y + OffSet.y,
						0);

					DebugExt.DrawCircle(point, Color.white, true, .2f);
				}

				if (colorCounter > Constants.Color.Length - 1)
					colorCounter = 0;
				else
					colorCounter++;
			}
		}

		/// <summary>
		///					*****   THIS IS AN UNSAFE METHOD   *****
		/// </summary>
		public void DrawMapBoundary() {
			if (Rooms.IsEmptyOrNull()) {
				return;
			}

			var colorCounter = 0;

			foreach (var record in MapData.TileHashset) {
				if (!record.IsMapBoundary)
					continue;

				var shiftedBorder = BorderSize / 2f;
				var shiftedX      = Mathf.CeilToInt(-NumOfRows  / 2f);
				var shiftedY      = Mathf.FloorToInt(-NumOfCols / 2f);

				var pos = new Vector3(
					record.Coordinate.x + shiftedX + shiftedBorder,
					record.Coordinate.y + shiftedY + shiftedBorder,
					0);

				DebugExt.DrawCircle(pos, Color.white, true, .2f);

				if (colorCounter > Constants.Color.Length - 1)
					colorCounter = 0;
				else
					colorCounter++;
			}
		}

		int DetermineHighestRoomCount() {
			int currentHighestCount = 0;

			foreach (var outline in MapData.RoomOutlines) {
				if (outline.Count > currentHighestCount)
					currentHighestCount = outline.Count;
			}

			return currentHighestCount;
		}

		Vector2 DeterminePosition(Room mainRoom) {
			EdgeTiles = mainRoom.EdgeTiles;
			var tiles = mainRoom.Tiles;
			var count = tiles.Count;

			var index = Random.Range(0, count - 1);
			var t     = tiles[index];

			return GridToDrawOn.CellToWorld(new Vector3Int(t.x, t.y, 0));
		}

		public DataProcessor(ProceduralConfig config, MapData mapData, IReadOnlyList<Room> rooms) {
			GridToDrawOn    = config.Grid;
			Rooms           = rooms;
			MapData         = mapData;
			OffSet          = config.EdgeColliderOffset;
			NumOfRows       = config.Rows;
			NumOfCols       = config.Columns;
			BorderSize      = config.BorderSize;
			BoundaryTilemap = config.TileMapDictionary[TileMapType.Boundary];
		}
	}
}