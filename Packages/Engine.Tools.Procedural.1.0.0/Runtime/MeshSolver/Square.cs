namespace Engine.Procedural.Runtime {
	public class Square {
		public int  BitwiseNodesSum;
		public Node CenterTop, CenterRight, CenterBottom, CenterLeft;

		public ControlNode TopLeft, TopRight, BottomRight, BottomLeft;

		public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft) {
			TopLeft      = topLeft;
			TopRight     = topRight;
			BottomLeft   = bottomLeft;
			BottomRight  = bottomRight;
			CenterTop    = topLeft.Right;
			CenterRight  = bottomRight.Above;
			CenterBottom = bottomLeft.Right;
			CenterLeft   = bottomLeft.Above;
			CalculateBitwiseSum(topLeft, topRight, bottomRight, bottomLeft);
		}

		void CalculateBitwiseSum(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight,
			ControlNode bottomLeft) {
			if (topLeft.IsWall) BitwiseNodesSum     += 8;
			if (topRight.IsWall) BitwiseNodesSum    += 4;
			if (bottomRight.IsWall) BitwiseNodesSum += 2;
			if (bottomLeft.IsWall) BitwiseNodesSum  += 1;
		}
	}
}