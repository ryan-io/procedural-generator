
using UnityEngine;

namespace ProceduralGeneration {
	internal class GridCharacteristicsSolver {
		Grid Grid { get; }
		GeneratorToolsCtx ToolsCtx { get; }
		string SerializationName { get; }
		
		internal void Set() {
			new RenameTilemapContainer().Rename(SerializationName, Grid.gameObject);
			var tools = new GeneratorTools(ToolsCtx);
			tools.SetOriginWrtMap(Grid.gameObject);
			tools.SetGridScale(Constants.Instance.CellSize);
		}
		
		public GridCharacteristicsSolver(GridCharacteristicsSolverCtx ctx, GeneratorToolsCtx toolsCtx) {
			Grid              = ctx.Grid;
			SerializationName = ctx.SerializationName;
			ToolsCtx          = toolsCtx;
		}
	}
}