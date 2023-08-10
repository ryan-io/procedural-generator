using UnityEngine;

namespace ProceduralGeneration {
	public class ContainerBuilder {
		GameObject Root { get; }

		public (TileMapDictionary dictionary, Grid grid) Build() {
			var container = BuildNewContainer();
			var grid      = AddGridComponent(container);

			var tilemapBuilder = new TilemapBuilder();
			var dict           = tilemapBuilder.Build(container);

			return new(dict, grid);
		}

		GameObject BuildNewContainer() {
			var obj = new GameObject("grid-container") {
				transform = { parent = Root.transform, position = Vector3.zero }
			};

			return obj;
		}

		Grid AddGridComponent(GameObject container) {
			var grid = container.AddComponent<Grid>();

			grid.cellGap     = Vector3.zero;
			grid.cellSize    = new Vector3(1, 1, 0);
			grid.cellLayout  = GridLayout.CellLayout.Rectangle;
			grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;

			return grid;
		}

		public ContainerBuilder(GameObject root) {
			Root = root;
		}
	}
}