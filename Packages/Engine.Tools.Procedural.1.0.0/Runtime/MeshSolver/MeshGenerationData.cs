using System;
using System.Collections.Generic;
using Engine.Procedural.ColliderSolver;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public class MeshGenerationData {
		[field: SerializeField] public Mesh               GeneratedMesh       { get; private set; }
		[field: SerializeField] public RoomMeshDictionary GeneratedRoomMeshes { get; private set; }
		[field: SerializeField] public List<Vector3>      Vertices            { get; private set; }
		[field: SerializeField] public List<int>          Triangles           { get; private set; }

		public MeshGenerationData(
			Mesh generatedMesh,
			RoomMeshDictionary generatedRoomMeshes,
			List<Vector3> vertices,
			List<int> triangles) {
			GeneratedMesh       = generatedMesh;
			GeneratedRoomMeshes = generatedRoomMeshes;
			Vertices            = vertices;
			Triangles           = triangles;
		}
	}
}