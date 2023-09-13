// ProceduralGeneration

using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace ProceduralGeneration {
	internal struct ProcessOffsetMapPositions : IJobParallelForBatch {
		[NativeDisableParallelForRestriction] [WriteOnly] public NativeArray<float3> OffsetPositions;

		[ReadOnly] public NativeArray<float3> MapPositions;
		[ReadOnly] public int                 CellSize;
		[ReadOnly] public float               OffsetX;
		[ReadOnly] public float               OffsetY;

		public void Execute(int startIndex, int count) {
			for (var i = startIndex; i < count; i++) {
				var p = MapPositions[i];
				OffsetPositions[i] = new float3(CellSize * p.x + OffsetX, CellSize * p.y + OffsetY, p.z);
			}
		}
	}
	
	[BurstCompile]
	internal class BurstPointer {
		unsafe delegate float OffsetPosDelegate(float3* mapPos, void* grid, int cellSize, float offsetX, float offsetY);

		[BurstCompile, MonoPInvokeCallback(typeof(OffsetPosDelegate))]
		internal static unsafe void OffsetPosPtr(float3* mapPos, void* grid, int cellSize, float offsetX, float offsetY) {
			*mapPos = new float3(cellSize * mapPos->x + offsetX, cellSize * mapPos->y + offsetY, mapPos->z);
		}
	}
}