using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.HighPerformance;
using UnityEngine;

namespace Engine.Procedural {
	public class FloodRegionRemovalSolver : RegionRemovalSolver {
		List<Room> Rooms                { get; set; }
		int        WallRemovalThreshold { get; }
		int        RoomRemovalThreshold { get; }
		int        MapWidth             { get; }
		int        MapHeight            { get; }

		public FloodRegionRemovalSolver(ProceduralConfig config) {
			_mapConnectionSolver = new MapConnectionSolver(config);
			Rooms                = new List<Room>();
			WallRemovalThreshold = config.WallRemovalThreshold;
			RoomRemovalThreshold = config.RoomRemovalThreshold;
			MapWidth             = config.Width;
			MapHeight            = config.Height;
		}

		public override void Remove(Span2D<int> map) {
			CullWalls(map);
			CullRooms(map);
		}

		void CullWalls(Span2D<int> map) {
			var regions = GetRegions(map, 1);

			foreach (var region in regions)
				if (region.Count < WallRemovalThreshold)
					foreach (var tile in region)
						map[tile.x, tile.y] = 0;
		}

		void CullRooms(Span2D<int> map) {
			var regions        = GetRegions(map, 0);
			var survivingRooms = new List<Room>();

			foreach (var region in regions)
				if (region.Count < RoomRemovalThreshold)
					foreach (var tile in region)
						map[tile.x, tile.y] = 1;
				else
					survivingRooms.Add(new Room(region, map));

			if (!survivingRooms.Any())
				return;

			survivingRooms.Sort();
			survivingRooms[0].SetIsMainRoom(true);
			survivingRooms[0].SetIsAccessibleToMainRoomDirect(true);

			_mapConnectionSolver.Connect(map, survivingRooms);
			Rooms = survivingRooms;
		}

		// TODO: Need to determine an appropriate stackalloc size for handling adding Regions to a span
		Region GetRegionTiles(Span2D<int> map, int startX, int startY) {
			var tiles    = new Region();
			var mapFlags = new bool[MapWidth, MapHeight];
			var tileType = map[startX, startY];

			var queue = new Queue<Vector2Int>();
			queue.Enqueue(new Vector2Int(startX, startY));
			mapFlags[startX, startY] = true;

			while (queue.Count > 0) {
				var tile = queue.Dequeue();
				tiles.Add(tile);
				for (var x = tile.x - 1; x <= tile.x + 1; x++) {
					for (var y = tile.y - 1; y <= tile.y + 1; y++)
						if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight
						    && (x == tile.x || y == tile.y))
							if (mapFlags[x, y] == false && map[x, y] == tileType) {
								mapFlags[x, y] = true;
								queue.Enqueue(new Vector2Int(x, y));
							}
				}
			}

			return tiles;
		}

		// TODO: Need to determine an appropriate stackalloc size for handling adding Regions to a span
		IEnumerable<Region> GetRegions(Span2D<int> map, int tileType) {
			var regions  = new List<Region>();
			var mapFlags = new Span2D<bool>(new bool[MapWidth, MapHeight]);

			for (var x = 0; x < MapWidth; x++) {
				for (var y = 0; y < MapHeight; y++)
					if (mapFlags[x, y] == false && map[x, y] == tileType) {
						var newRegion = GetRegionTiles(map, x, y);
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