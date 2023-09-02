using UnityEngine;

namespace ProceduralGeneration {
	public class ControlNode : Node {
		public readonly Node Above;
		public readonly Node Right;
		public readonly bool IsWall;

		public ControlNode(Vector2 position, bool isWall, float squareSize) : base(position) {
			IsWall = isWall;
			Above  = new Node(position + Vector2.up    * squareSize / 2f);
			Right  = new Node(position + Vector2.right * squareSize / 2f);
		}
	}
}