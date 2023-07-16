// Algorthims

using UnityEngine;

namespace Engine.Procedural {
	public class Node {
		public Vector3 Position;
		public int     VertexIndex = -1;

		public Node(Vector2 position) => Position = position;
	}
}