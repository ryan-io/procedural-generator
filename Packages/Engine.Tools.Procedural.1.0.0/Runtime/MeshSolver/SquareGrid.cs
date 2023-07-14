using Engine.Procedural.Nodes;
using UnityEngine;

namespace Engine.Procedural {
	public class SquareGrid {
		public Square[,] Squares { get; private set; }

		public void SetSquares(int[,] map) {
			var nodeCountX   = map.GetLength(0);
			var nodeCountY   = map.GetLength(1);
			var tileWidth    = nodeCountX * Constants.CELL_SIZE;
			var tileHeight   = nodeCountY * Constants.CELL_SIZE;
			var controlNodes = new ControlNode[nodeCountX, nodeCountY];

			for (var i = 0; i < nodeCountX; i++) {
				for (var j = 0; j < nodeCountY; j++) {
					var position = new Vector2(
						-tileWidth  / 2f + i * Constants.CELL_SIZE + Constants.CELL_SIZE / 2f,
						-tileHeight / 2f + j * Constants.CELL_SIZE + Constants.CELL_SIZE / 2f);
					controlNodes[i, j] = new ControlNode(position, map[i, j] == 1, Constants.CELL_SIZE);
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