using System;
using System.Collections.Generic;
using System.Linq;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	[Serializable]
	public class MapData {
		[field: SerializeField] public TileHashset                    TileHashset          { get; private set; }
		[field: SerializeField] public Mesh                           Mesh                 { get; private set; }
		[field: SerializeField] public List<Vector3>                  MeshVertices         { get; private set; }
		[field: SerializeField] public List<int>                      MeshTriangles        { get; private set; }
		[field: SerializeField] public List<List<int>>                RoomOutlines         { get; private set; }
		[field: SerializeField] public List<Vector3>                  TilePositionsShifted { get; set; }
		[field: SerializeField] public Dictionary<int, List<Vector3>> BoundaryCorners      { get; set; }
		[field: SerializeField] public RoomMeshDictionary             RoomMeshDictionary   { get; private set; }

		public Dictionary<int, List<SerializableVector3>> GetBoundaryCoords() {
			if (BoundaryCorners.IsEmptyOrNull())
				return default;

			var dict = new Dictionary<int, List<SerializableVector3>>();
			var index = BoundaryCorners.Keys.First();

			foreach (var corner in BoundaryCorners) {
				var serializedList = corner.Value.AsSerialized().ToList();
				dict.Add(index, serializedList);
				index++;
			}

			return dict;
		}

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