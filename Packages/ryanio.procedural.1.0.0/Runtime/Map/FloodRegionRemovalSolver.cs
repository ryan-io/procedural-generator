using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.HighPerformance;
using UnityEngine;

namespace ProceduralGeneration {
	internal class FloodFillRegionSolver : RegionSolver {
		int WallRemovalThreshold { get; }
		int RoomRemovalThreshold { get; }
		int Rows                 { get; set; }
		int Columns              { get; set; }

		internal FloodFillRegionSolver(RemoveRegionsSolverCtx ctx) {
			_mapConnectionSolver = new MapConnectionSolver(ctx);
			WallRemovalThreshold = ctx.WallRemoveThreshold;
			RoomRemovalThreshold = ctx.RoomRemoveThreshold;
		}

		internal override List<Room> Remove(ref int[,] map) {
			Rows    = map.GetLength(0);
			Columns = map.GetLength(1);

			// var copy = new Span2D<int>(new int[Rows, Columns]);
			// primarySpan.CopyTo(copy);

			CullWalls(ref map);
			return CullRooms(ref map);
		}

		void CullWalls(ref int[,] map) {
			var regions = GetRegions(ref map, 1);

			foreach (var region in regions)
				if (region.Count() < WallRemovalThreshold)
					foreach (var tile in region)
						map[tile.x, tile.y] = 0;
		}

		List<Room> CullRooms(ref int[,] map) {
			var regions        = GetRegions(ref map, 0);
			var survivingRooms = new List<Room>();

			foreach (var region in regions)
				if (region.Count < RoomRemovalThreshold)
					foreach (var tile in region)
						map[tile.x, tile.y] = 1;
				else
					survivingRooms.Add(new Room(region, ref map));

			if (!survivingRooms.Any()) {
				return Enumerable.Empty<Room>().ToList();
			}

			survivingRooms.Sort();
			survivingRooms[0].SetIsMainRoom(true);
			survivingRooms[0].SetIsAccessibleToMainRoomDirect(true);

			_mapConnectionSolver.Connect(ref map, survivingRooms);
			return survivingRooms;
		}

		// TODO: Need to determine an appropriate stackalloc size for handling adding Regions to a span
		Region GetRegionTiles(ref int[,] map, int startX, int startY) {
			var tiles    = new Region(); // List<Vector2Int>
			var mapFlags = new bool[Rows, Columns];
			var tileType = map[startX, startY];
			var queue    = new Queue<Vector2Int>();

			queue.Enqueue(new Vector2Int(startX, startY));
			mapFlags[startX, startY] = true;

			while (queue.Count > 0) {
				var tile = queue.Dequeue();
				tiles.Add(tile);

				// flood-fill
				for (var x = tile.x - 1; x <= tile.x + 1; x++) {
					for (var y = tile.y - 1; y <= tile.y + 1; y++)
						if ((x >= 0 && x < Rows && y >= 0 && y < Columns) && (x == tile.x || y == tile.y))
							if (mapFlags[x, y] == false && map[x, y] == tileType) {
								mapFlags[x, y] = true;
								queue.Enqueue(new Vector2Int(x, y));
							}
				}
			}

			return tiles;
		}

		// TODO: Need to determine an appropriate stackalloc size for handling adding Regions to a span
		IEnumerable<Region> GetRegions(ref int[,] map, int tileType) {
			var regions       = new List<Region>();
			var mapFlagsArray = new bool[Rows, Columns];
			var mapFlags      = mapFlagsArray.AsSpan2D();

			for (var x = 0; x < Rows; x++) {
				for (var y = 0; y < Columns; y++)
					if (mapFlags[x, y] == false && map[x, y] == tileType) {
						var newRegion = GetRegionTiles(ref map, x, y);
						regions.Add(newRegion);

						foreach (var tile in newRegion)
							mapFlags[tile.x, tile.y] = true;
					}
			}

			return regions;
		}

		readonly MapConnectionSolver _mapConnectionSolver;
	}
}