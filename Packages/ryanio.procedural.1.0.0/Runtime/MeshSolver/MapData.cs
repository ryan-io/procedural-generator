using System;
using System.Collections.Generic;
using System.Linq;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	[Serializable]
	internal class MapData {
		[field: SerializeField] public int[,] Map { get; private set; }
		
		[field: SerializeField] internal TileHashset                    TileHashset          { get; set; }
		[field: SerializeField] internal Mesh                           Mesh                 { get; set; }
		[field: SerializeField] internal List<List<int>>                RoomOutlines         { get; set; }

		internal MapData() {
		}

		internal MapData(int[,] map, TileHashset tileHashset, MeshData meshCollisionData) {
			Map          = map;
			TileHashset  = tileHashset;
			Mesh         = meshCollisionData.Mesh;
			RoomOutlines = meshCollisionData.RoomOutlines;
		}
	}
}