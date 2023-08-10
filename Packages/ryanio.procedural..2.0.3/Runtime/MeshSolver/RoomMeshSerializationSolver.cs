using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	public static class RoomMeshSerializationSolver {
		public static RoomMeshCollectionData GenerateMeshes(RoomMeshSerializationSolverModel model) {
			var collection = new RoomMeshCollectionData();

			for (var i = 0; i < model.Outlines.Count; i++) {
				var outline  = model.Outlines[i];
				var vertices = new List<Vector3>();

				for (var j = 0; j < outline.Count; j++)
					vertices.Add(model.WalkableVertices[outline[j]]);

				var roomMeshData = new RoomMeshData(vertices, outline);
				collection.Add(i, roomMeshData);
			}

			return collection;
		}
	}
}