// ProceduralGeneration

using JacksonDunstan.NativeCollections;
using Unity.Burst;
using Unity.Jobs;

namespace ProceduralGeneration {
	[BurstCompile]
	internal struct SetNewWallValueJob : IJob {
		public NativeArray2D<int> NativeMapCopy;
		public int                Count;
		public int                CurrentRow;
		public int                CurrentCol;
		
		public void Execute() {
			if (Count > COMPARISON_LIMIT)
				NativeMapCopy[CurrentRow, CurrentCol] = 1;

			else if (Count < COMPARISON_LIMIT)
				NativeMapCopy[CurrentRow, CurrentCol] = 0;

		}
		
		const int COMPARISON_LIMIT = 4;
	}
}