// Engine.Procedural

using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct MeshSolverData {
		internal Mesh               Mesh               { get; }
		internal List<Vector3>      MeshVertices       { get; }
		internal List<int>          MeshTriangles      { get; }
		internal List<List<int>>    RoomOutlines       { get; }
		internal RoomMeshDictionary RoomMeshDictionary { get; }

		internal MeshSolverData(
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