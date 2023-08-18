using System;
using System.Collections.Generic;
using System.Linq;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	[Serializable]
	internal class MapData {
		[field: SerializeField] internal TileHashset                    TileHashset          { get; set; }
		[field: SerializeField] internal Mesh                           Mesh                 { get; set; }
		[field: SerializeField] internal List<Vector3>                  MeshVertices         { get; set; }
		[field: SerializeField] internal List<int>                      MeshTriangles        { get; set; }
		[field: SerializeField] internal List<List<int>>                RoomOutlines         { get; set; }
		[field: SerializeField] internal List<Vector3>                  TilePositionsShifted { get; set; }
		[field: SerializeField] internal Dictionary<int, List<Vector3>> SpriteBoundaryCoords { get; set; }
		[field: SerializeField] internal Dictionary<int, List<Vector3>> ColliderCoords       { get; set; }
		[field: SerializeField] internal RoomMeshDictionary             RoomMeshDictionary   { get; set; }

		// internal Dictionary<int, List<SerializableVector3>> GetBoundaryCoords() {
		// 	if (SpriteBoundaryCoords.IsEmptyOrNull())
		// 		return default;
		//
		// 	var dict  = new Dictionary<int, List<SerializableVector3>>();
		// 	var index = SpriteBoundaryCoords.Keys.First();
		//
		// 	foreach (var corner in SpriteBoundaryCoords) {
		// 		var serializedList = corner.Value.AsSerialized().ToList();
		// 		dict.Add(index, serializedList);
		// 		index++;
		// 	}
		//
		// 	return dict;
		// }

		internal MapData() {
		}

		internal MapData(TileHashset tileHashset, MeshData meshCollisionData) {
			TileHashset        = tileHashset;
			Mesh               = meshCollisionData.Mesh;
			RoomOutlines       = meshCollisionData.RoomOutlines;
			RoomMeshDictionary = meshCollisionData.RoomMeshDictionary;
			MeshVertices       = meshCollisionData.MeshVertices;
			MeshTriangles      = meshCollisionData.MeshTriangles;
		}
	}
}