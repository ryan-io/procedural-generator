using System;
using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralGeneration {
	[Serializable]
	public class Room : IComparable<Room> {
		[ShowInInspector] [ReadOnly] [SerializeField]
		Region _tiles;

		[ShowInInspector] [ReadOnly] [SerializeField]
		Region _edgeTiles;

		[ShowInInspector] [ReadOnly] [SerializeField]
		int _size;

		[ShowInInspector] [ReadOnly] [SerializeField]
		bool _isAccessibleToMainRoom;

		[ShowInInspector] [ReadOnly] [SerializeField]
		bool _isMainRoom;

		public Room() {
		}

		public Room(Region tiles, Span2D<int> map) {
			_tiles         = tiles;
			_size          = tiles.Count;
			_edgeTiles     = new Region();
			ConnectedRooms = new List<Room>();

			foreach (var tile in _tiles) {
				for (var x = tile.x - 1; x < tile.x + 1; x++) {
					for (var y = tile.y - 1; y < tile.y + 1; y++)
						if (x == tile.x || y == tile.y)
							if (map[x, y] == 1)
								_edgeTiles.Add(tile);
				}
			}
		}

		[HideInInspector] public List<Room> ConnectedRooms { get; }

		public Region Tiles                  => _tiles;
		public Region EdgeTiles              => _edgeTiles;
		public int    Size                   => _size;
		public bool   IsAccessibleToMainRoom => _isAccessibleToMainRoom;
		public bool   IsMainRoom             => _isMainRoom;

		public int CompareTo(Room other) => other._size.CompareTo(_size);

		public static void ConnectRooms(Room a, Room b) {
			if (a._isAccessibleToMainRoom)
				b.SetAccessibleFromMainRoom();
			
			else if (b._isAccessibleToMainRoom) 
				a.SetAccessibleFromMainRoom();

			a.ConnectedRooms.Add(b);
			b.ConnectedRooms.Add(a);
		}

		public bool IsConnected(Room room) => ConnectedRooms.Contains(room);

		public void SetIsMainRoom(bool state)                   => _isMainRoom = state;
		public void SetIsAccessibleToMainRoomDirect(bool state) => _isAccessibleToMainRoom = state;

		public void SetAccessibleFromMainRoom() {
			if (!_isAccessibleToMainRoom) {
				_isAccessibleToMainRoom = true;
				foreach (var room in ConnectedRooms) room.SetAccessibleFromMainRoom();
			}
		}
	}
}