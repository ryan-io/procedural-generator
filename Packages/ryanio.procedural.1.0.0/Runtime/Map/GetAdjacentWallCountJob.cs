// ProceduralGeneration

using System;
using Unity.Jobs;

namespace ProceduralGeneration {
	internal struct FillMapJob : IJobParallelFor, IDisposable {
		public void Execute(int index) {
			throw new NotImplementedException();
		}

		public void Dispose() {
			
		}
	}
	//
	// [BurstCompile]
	// internal struct IsWithinBoundaryJob : IJob, IDisposable {
	// 	
	// }

	// internal struct GetAdjacentWallCountJob : IJobFor, IDisposable {
	// 	[ReadOnly, NativeDisableParallelForRestriction] public NativeArray2D<int> NativeMap;
	//
	// 	public int Rows;
	// 	public int Cols;
	// 	public int CurrentRow;
	// 	public int CurrentCol;
	// 	public int Count;
	//
	// 	public void Execute(int index) {
	// 		Count = 0;
	//
	// 		for (var neighborX = CurrentRow - 1; neighborX <= CurrentRow + 1; neighborX++) {
	// 			for (var neighborY = CurrentCol - 1; neighborY <= CurrentCol + 1; neighborY++)
	// 				if (IsWithinBoundary(neighborX, neighborY)) {
	// 					if (neighborX != CurrentRow || neighborY != CurrentCol)
	// 						Count += NativeMap[neighborX, neighborY];
	// 				}
	//
	// 				else {
	// 					Count++;
	// 				}
	// 		}
	// 	}
	//
	// 	bool IsWithinBoundary(int neighborX, int neighborY) {
	// 		return neighborX > BORDER_SAFETY_FACTOR        &&
	// 		       neighborX < Rows - BORDER_SAFETY_FACTOR &&
	// 		       neighborY > BORDER_SAFETY_FACTOR        &&
	// 		       neighborY < Cols - BORDER_SAFETY_FACTOR;
	// 	}
	//
	// 	public void Dispose() {
	// 		NativeMap.Dispose();
	// 	}
	//
	// 	const  int  BORDER_SAFETY_FACTOR = 4;
	// }
}