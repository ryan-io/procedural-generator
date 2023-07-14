using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural.ColliderSolver {
	public readonly struct RoomMeshSerializationSolverModel {
		public List<List<int>> Outlines         { get; }
		public List<Vector3>   WalkableVertices { get; }

		public RoomMeshSerializationSolverModel(List<List<int>> outlines, List<Vector3> walkableVertices) {
			Outlines         = outlines;
			WalkableVertices = walkableVertices;
		}
	}
}