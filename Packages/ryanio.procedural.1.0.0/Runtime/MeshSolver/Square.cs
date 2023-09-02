namespace ProceduralGeneration {
	public class Square {
		public          int  BitwiseNodesSum;
		
		public readonly Node CenterTop;
		public readonly Node CenterRight;
		public readonly Node CenterBottom;
		public readonly Node CenterLeft;

		public readonly ControlNode TopLeft;
		public readonly ControlNode TopRight;
		public readonly ControlNode BottomRight;
		public readonly ControlNode BottomLeft;

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
			if (topLeft.IsWall) BitwiseNodesSum     += 1 <<3;
			if (topRight.IsWall) BitwiseNodesSum    += 1 <<2;
			if (bottomRight.IsWall) BitwiseNodesSum += 1 << 1;
			if (bottomLeft.IsWall) BitwiseNodesSum  += 1 << 0;
		}
	}
}