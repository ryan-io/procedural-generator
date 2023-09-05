using CommunityToolkit.HighPerformance;
using UnityEngine;

namespace ProceduralGeneration {
	public class SquareGrid {
		public Square[,] Squares { get; private set; }

		public void SetSquares(Span2D<int> map) {
			var nodeCountX   = map.Height;
			var nodeCountY   = map.Width;
			var tileWidth    = nodeCountX * Constants.Instance.CellSize;
			var tileHeight   = nodeCountY * Constants.Instance.CellSize;
			var controlNodes = new ControlNode[nodeCountX, nodeCountY];

			for (var i = 0; i < nodeCountX; i++) {
				for (var j = 0; j < nodeCountY; j++) {
					var position = new Vector2(
						-tileWidth  / 2f + i * Constants.Instance.CellSize + Constants.Instance.CellSize / 2f,
						-tileHeight / 2f + j * Constants.Instance.CellSize + Constants.Instance.CellSize / 2f);
					controlNodes[i, j] = new ControlNode(position, map[i, j] == 1, Constants.Instance.CellSize);
				}
			}  

			Squares = new Square[nodeCountX - 1, nodeCountY - 1];

			for (var i = 0; i < nodeCountX - 1; i++) {
				for (var j = 0; j < nodeCountY - 1; j++)
					Squares[i, j] = new Square(
						controlNodes[i, j + 1],
						controlNodes[i    + 1, j + 1],
						controlNodes[i    + 1, j],
						controlNodes[i, j]);
			}
			
		}

		public SquareGrid() {
		}
	}
}