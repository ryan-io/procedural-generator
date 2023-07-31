using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	[Serializable]
	public class RoomMeshDictionary : Dictionary<int, Mesh> {
		public RoomMeshDictionary(RoomMeshCollectionData data) {
			var counter = 1;

			foreach (var pair in data) {
				var mesh = new Mesh {
					name      = $"Room {counter}",
					vertices  = pair.Value.Vertices.ToArray(),
					triangles = pair.Value.Triangles.ToArray()
				};

				Add(pair.Key, mesh);
				counter++;
			}
		}

		public RoomMeshDictionary() {
		}
	}
}