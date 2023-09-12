using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralGeneration {
	
	
	internal struct ProcessOffsetMapPositions : IJobParallelFor {
		[NativeDisableParallelForRestriction] public NativeArray<float3> OffsetPositions;

		[ReadOnly] public NativeArray<float3> MapPositions;
		[ReadOnly] public int                 CellSize;
		[ReadOnly] public float               OffsetX;
		[ReadOnly] public float               OffsetY;

		public void Execute(int index) {
			var p = MapPositions[index];
			OffsetPositions[index] = new float3(CellSize * p.x + OffsetX, CellSize * p.y + OffsetY, p.z);
		}
	}

	internal class GridCharacteristicsSolver {
		Grid              Grid              { get; }
		GeneratorToolsCtx ToolsCtx          { get; }
		string            SerializationName { get; }

		internal void Set() {
			new RenameTilemapContainer().Rename(SerializationName, Grid.gameObject);
			var tools = new GeneratorTools(ToolsCtx);
			//tools.SetOriginWrtMap(Grid.gameObject);
			tools.SetGridScale(Constants.Instance.CellSize);
		}

		public GridCharacteristicsSolver(GridCharacteristicsSolverCtx ctx, GeneratorToolsCtx toolsCtx) {
			Grid              = ctx.Grid;
			SerializationName = ctx.SerializationName;
			ToolsCtx          = toolsCtx;
		}
	}
}