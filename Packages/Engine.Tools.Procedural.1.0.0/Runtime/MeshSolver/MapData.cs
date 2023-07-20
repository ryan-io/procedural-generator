using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public class MapData {
		[field: SerializeField] public TileHashset        TileHashset        { get; private set; }
		[field: SerializeField] public Mesh               Mesh               { get; private set; }
		[field: SerializeField] public List<Vector3>      MeshVertices       { get; private set; }
		[field: SerializeField] public List<int>          MeshTriangles      { get; private set; }
		[field: SerializeField] public List<List<int>>    RoomOutlines       { get; private set; }
		[field: SerializeField] public RoomMeshDictionary RoomMeshDictionary { get; private set; }

		public MapData(TileHashset tileHashset, MeshSolverData meshCollisionData) {
			TileHashset        = tileHashset;
			Mesh               = meshCollisionData.Mesh;
			RoomOutlines       = meshCollisionData.RoomOutlines;
			RoomMeshDictionary = meshCollisionData.RoomMeshDictionary;
			MeshVertices       = meshCollisionData.MeshVertices;
			MeshTriangles      = meshCollisionData.MeshTriangles;
		}
	}
}