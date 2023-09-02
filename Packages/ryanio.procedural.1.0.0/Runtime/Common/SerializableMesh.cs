// ProceduralGeneration

using System;
using System.Collections.Generic;
using System.Linq;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	[Serializable]
	public struct SerializableMesh {
		public static implicit operator Mesh(SerializableMesh serV) => new() {
			triangles = serV.Triangles,
			uv        = serV.Uvs.Deserialized().ToArray(),
			vertices  = serV.Vertices.Deserialized().ToArray()
		};

		public static explicit operator SerializableMesh(Mesh v) => new() {
			Triangles = v.triangles,
			Uvs       = v.uv.AsSerialized().ToArray(),
			Vertices  = v.vertices.AsSerialized().ToArray()
		};

		public SerializableVector3[] Vertices  { get; set; }
		public SerializableVector2[] Uvs       { get; set; }
		public int[]                 Triangles { get; set; }

		public SerializableMesh(Mesh mesh) {
			Vertices  = SerializeVertices(mesh.vertices);
			Uvs       = SerializeUvs(mesh.uv);
			Triangles = mesh.triangles;
		}

		public SerializableMesh(Vector3[] vertices, Vector2[] uvs, int[] triangles) {
			Vertices  = SerializeVertices(vertices);
			Uvs       = SerializeUvs(uvs);
			Triangles = triangles;
		}

		public SerializableMesh(SerializableVector3[] vertices, SerializableVector2[] uvs, int[] triangles) {
			Vertices  = vertices;
			Uvs       = uvs;
			Triangles = triangles;
		}

		static SerializableVector2[] SerializeUvs(IReadOnlyList<Vector2> uv) {
			var serUv = new SerializableVector2[uv.Count];

			for (var i = 0; i < uv.Count; i++) {
				serUv[i] = (SerializableVector2)uv[i];
			}

			return serUv;
		}

		static SerializableVector3[] SerializeVertices(IReadOnlyList<Vector3> v) {
			var serV = new SerializableVector3[v.Count];

			for (var i = 0; i < v.Count; i++) {
				serV[i] = (SerializableVector3)v[i];
			}

			return serV;
		}
	}
}