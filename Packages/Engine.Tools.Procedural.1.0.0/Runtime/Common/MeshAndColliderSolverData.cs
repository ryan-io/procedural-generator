// Engine.Procedural

using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural {
	public readonly struct MeshAndColliderSolverData {
		public Mesh               Mesh               { get; }
		public List<Vector3>      MeshVertices       { get; }
		public List<int>          MeshTriangles      { get; }
		public List<List<int>>    RoomOutlines       { get; }
		public RoomMeshDictionary RoomMeshDictionary { get; }

		public MeshAndColliderSolverData(
			Mesh mesh,
			List<Vector3> meshVertices,
			List<int> meshTriangles,
			List<List<int>> roomOutlines,
			RoomMeshDictionary roomMeshDictionary) {
			Mesh               = mesh;
			MeshVertices       = meshVertices;
			MeshTriangles      = meshTriangles;
			RoomOutlines       = roomOutlines;
			RoomMeshDictionary = roomMeshDictionary;
		}
	}
}