using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class ControlNode : Node {
		public Node Above, Right;
		public bool IsWall;

		public ControlNode(Vector2 position, bool isWall, float squareSize) : base(position) {
			IsWall = isWall;
			Above  = new Node(position + Vector2.up    * squareSize / 2f);
			Right  = new Node(position + Vector2.right * squareSize / 2f);
		}
	}
}