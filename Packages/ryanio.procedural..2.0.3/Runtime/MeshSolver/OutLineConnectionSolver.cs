using System.Collections.Generic;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public static class OutLineConnectionSolver {
		public static void Solve(List<Vector3> verticesWalkable, HashSet<int> checkedVertices,
			List<List<int>> outlines, Dictionary<int, List<Triangle>> triangleTracker) {
			var count = verticesWalkable.Count;

			for (var i = 0; i < count; i++)
				if (!checkedVertices.Contains(i)) {
					var newOutlineVertex = GetConnectedOutlineVertex(i, checkedVertices, triangleTracker);
					if (newOutlineVertex != -1) {
						checkedVertices.Add(i);

						var newOutline = new List<int>();
						outlines.Add(newOutline);
						FollowOutline(newOutlineVertex, outlines.Count - 1, checkedVertices, outlines, triangleTracker);
						outlines[^1].Add(i);
					}
				}

#if UNITY_STANDALONE || UNITY_EDITOR
			// var log = new UnityLogging();
			// log.Msg($"Total rooms generated: {outlines.Count}", "Total Rooms Created", italic: true, bold: true);
#endif
		}

		static void FollowOutline(int vertexIndex, int outlineIndex, HashSet<int> checkedVertices,
			List<List<int>> outlines, Dictionary<int, List<Triangle>> triangleTracker) {
			
			while (true) { // RECURSIVE FUNCTION
				outlines[outlineIndex].Add(vertexIndex);
				checkedVertices.Add(vertexIndex);
				var nextVertex = GetConnectedOutlineVertex(vertexIndex, checkedVertices, triangleTracker);

				if (nextVertex != -1) {
					vertexIndex = nextVertex;
					continue;
				}

				break;
			}
		}

		static int GetConnectedOutlineVertex(
			int vertexIndex, 
			ICollection<int> checkedVertices, 
			Dictionary<int, List<Triangle>> triangleTracker) {  
			var triangleList = triangleTracker[vertexIndex];
			var count        = triangleList.Count;

			for (var i = 0; i < count; i++) {
				var triangle = triangleList[i];

				for (var j = 0; j < 3; j++) {
					var vertexB = triangle[j];

					//&& !checkedVertices.Contains(vertexB) -> this is required. 
					if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
						if (IsOutlineEdge(vertexIndex, vertexB, triangleTracker))
							return vertexB;
				}
			}

			return -1;
		}

		static bool IsOutlineEdge(int vertexA, int vertexB, Dictionary<int, List<Triangle>> triangleTracker) {
			var trianglesContainingVertexA = triangleTracker[vertexA];
			var count                      = trianglesContainingVertexA.Count;
			var sharedTriangleCount        = 0;

			for (var i = 0; i < count; i++) {
				if (trianglesContainingVertexA[i].Contains(vertexB)) {
					sharedTriangleCount++;
				if (sharedTriangleCount > 1)
					break;
				}

			}

			return sharedTriangleCount == 1;
		}
	}
}