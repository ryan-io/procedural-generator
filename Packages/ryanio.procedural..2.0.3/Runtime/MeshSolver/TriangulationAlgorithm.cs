using System.Collections.Generic;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class TriangulationAlgorithm {
		public TriangulationAlgorithm() {
			GetWalkableTriangles = new List<int>();
			GetWalkableVertices  = new List<Vector3>();
		}

		public List<int> GetWalkableTriangles { get; }

		public List<Vector3> GetWalkableVertices { get; }

		public void TriangulateSquare(
			Square square, HashSet<int> checkedVertices, Dictionary<int, List<Triangle>> triangleTracker) {
			switch (square.BitwiseNodesSum) {
				case 0: break;
				case 1:
					MeshFromPoints(triangleTracker, square.CenterLeft, square.CenterBottom, square.BottomLeft);
					break;
				case 2:
					MeshFromPoints(triangleTracker, square.BottomRight, square.CenterBottom, square.CenterRight);
					break;
				case 3:
					MeshFromPoints(triangleTracker, square.CenterRight, square.BottomRight, square.BottomLeft,
						square.CenterLeft);
					break;
				case 4:
					MeshFromPoints(triangleTracker, square.TopRight, square.CenterRight, square.CenterTop);
					break;
				case 5:
					MeshFromPoints(triangleTracker, square.CenterTop, square.TopRight, square.CenterRight,
						square.CenterBottom,
						square.BottomLeft, square.CenterLeft);
					break;
				case 6:
					MeshFromPoints(triangleTracker, square.CenterTop, square.TopRight, square.BottomRight,
						square.CenterBottom);
					break;
				case 7:
					MeshFromPoints(triangleTracker, square.CenterTop, square.TopRight, square.BottomRight,
						square.BottomLeft,
						square.CenterLeft);
					break;
				case 8:
					MeshFromPoints(triangleTracker, square.TopLeft, square.CenterTop, square.CenterLeft);
					break;
				case 9:
					MeshFromPoints(triangleTracker, square.TopLeft, square.CenterTop, square.CenterBottom,
						square.BottomLeft);
					break;
				case 10:
					MeshFromPoints(triangleTracker, square.TopLeft, square.CenterTop, square.CenterRight,
						square.BottomRight,
						square.CenterBottom, square.CenterLeft);
					break;
				case 11:
					MeshFromPoints(triangleTracker, square.TopLeft, square.CenterTop, square.CenterRight,
						square.BottomRight,
						square.BottomLeft);
					break;
				case 12:
					MeshFromPoints(triangleTracker, square.TopLeft, square.TopRight, square.CenterRight,
						square.CenterLeft);
					break;
				case 13:
					MeshFromPoints(triangleTracker, square.TopRight, square.CenterRight, square.CenterBottom,
						square.BottomLeft,
						square.TopLeft);
					break;
				case 14:
					MeshFromPoints(triangleTracker, square.TopLeft, square.TopRight, square.BottomRight,
						square.CenterBottom,
						square.CenterLeft);
					break;
				case 15:
					MeshFromPoints(triangleTracker, square.TopLeft, square.TopRight, square.BottomRight,
						square.BottomLeft);
					checkedVertices.Add(square.TopLeft.VertexIndex);
					checkedVertices.Add(square.TopRight.VertexIndex);
					checkedVertices.Add(square.BottomRight.VertexIndex);
					checkedVertices.Add(square.BottomLeft.VertexIndex);
					break;
			}
		}

		void MeshFromPoints(IDictionary<int, List<Triangle>> triangleTracker, params Node[] points) {
			AssignVertices(points);
			if (points.Length >= 3) CreateTriangle(points[0], points[1], points[2], triangleTracker);
			if (points.Length >= 4) CreateTriangle(points[0], points[2], points[3], triangleTracker);
			if (points.Length >= 5) CreateTriangle(points[0], points[3], points[4], triangleTracker);
			if (points.Length >= 6) CreateTriangle(points[0], points[4], points[5], triangleTracker);
		}

		void AssignVertices(Node[] points) {
			var length = points.Length;

			for (var i = 0; i < length; i++)
				if (points[i].VertexIndex == -1) {
					points[i].VertexIndex = GetWalkableVertices.Count;
					GetWalkableVertices.Add(points[i].Position);
				}
		}

		void CreateTriangle(Node a, Node b, Node c, IDictionary<int, List<Triangle>> triangleTracker) {
			GetWalkableTriangles.Add(a.VertexIndex);
			GetWalkableTriangles.Add(b.VertexIndex);
			GetWalkableTriangles.Add(c.VertexIndex);

			var triangle = new Triangle(a.VertexIndex, b.VertexIndex, c.VertexIndex);
			TrackTriangle(triangle.vertexIndexA, triangle, triangleTracker);
			TrackTriangle(triangle.vertexIndexB, triangle, triangleTracker);
			TrackTriangle(triangle.vertexIndexC, triangle, triangleTracker);
		}

		void TrackTriangle(int vertexKey, Triangle triangle, IDictionary<int, List<Triangle>> triangleTracker) {
			if (triangleTracker.TryGetValue(vertexKey, out var value))
				value.Add(triangle);

			else
				triangleTracker.Add(vertexKey, new List<Triangle> { triangle });
		}
	}
}

// ----------------------------------------------------------------------------------------->UV's<
/*
 * If we need to assign UVs:
 * 
 * 	void AssignUV(int[,] map) {
		var width   = map.GetLength(0);
		var height  = map.GetLength(1);
		var topLeft = new Vector2(-width / 2f, -height / 2f);
		var size    = new Vector2(width,       height);
		var uvs     = new Vector2[_verticesWalkable.Count];
		var length  = uvs.Length;

		for (var i = 0; i < uvs.Length; i++) {
			var         pos      = new Vector2(_verticesWalkable[i].x, _verticesWalkable[i].z);
			const float tiling   = 1;
			var         percentX = Mathf.InverseLerp(topLeft.x, topLeft.x + size.x, pos.x) * tiling;
			var         percentY = Mathf.InverseLerp(topLeft.y, topLeft.y + size.y, pos.y) * tiling;

			uvs[i] = new Vector2(percentX % 1f, percentY % 1f);
		}

		GeneratedMesh.uv = uvs;
		Debug.Log(GeneratedMesh.uv.Length);
		GeneratedMesh.RecalculateNormals();
	}
 */