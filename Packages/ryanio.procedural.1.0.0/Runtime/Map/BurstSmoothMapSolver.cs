// ProceduralGeneration

using System;
using JacksonDunstan.NativeCollections;
using Unity.Collections;
using Unity.Jobs;

namespace ProceduralGeneration {
	internal class BurstSmoothMapSolver : SmoothMapSolver, IDisposable {
		int SmoothingIterations { get; }

		internal override void Smooth(ref int[,] map, Dimensions dimensions) {
			var numRows = map.GetLength(0);
			var numCols = map.GetLength(1);

			_mapNativeOriginal = new NativeArray2D<int>(map, Allocator.Persistent);
			_mapNativeModified = new NativeArray2D<int>(map, Allocator.Persistent);

			_rows = new NativeArray<int>(1, Allocator.Persistent) {
				[0] = numRows
			};
			_cols = new NativeArray<int>(1, Allocator.Persistent) {
				[0] = numCols
			};

			var handle = default(JobHandle);

			try {
				for (var i = 0; i < SmoothingIterations; i++) {
					var smoothJob = new SmoothMapJobCombined {
						MapNativeOriginal = _mapNativeOriginal,
						MapNativeModified = _mapNativeModified,
						Rows              = _rows,
						Cols              = _cols
					};

					var smoothJobHandle = smoothJob.Schedule(handle);

					while (true) {
						if (smoothJobHandle.IsCompleted)
							break;
					}

					smoothJobHandle.Complete();
					_mapNativeModified.CopyTo(_mapNativeOriginal);
					_mapNativeOriginal.CopyTo(map);
				}
			}
			catch {
				// ignored
			}
			finally {
				Dispose();
			}
		}

		public BurstSmoothMapSolver(SmoothMapSolverCtx ctx) {
			SmoothingIterations = ctx.SmoothingIterations;
		}

		public void Dispose() {
			_mapNativeOriginal.Dispose();
			_mapNativeModified.Dispose();
			_rows.Dispose();
			_cols.Dispose();
		}

		NativeArray2D<int> _mapNativeOriginal;
		NativeArray2D<int> _mapNativeModified;
		NativeArray<int>   _rows;
		NativeArray<int>   _cols;
	}
}