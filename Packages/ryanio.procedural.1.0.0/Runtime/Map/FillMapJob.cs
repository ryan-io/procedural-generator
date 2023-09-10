// // ProceduralGeneration
//
// using System;
// using BCL;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Jobs;
// using Random = Unity.Mathematics.Random;
//
// namespace ProceduralGeneration {
// 	[BurstCompile]
// 	internal struct FillMapJob : IJobParallelFor, IDisposable {
// 		public     NativeArray<int> Output;
// 		[ReadOnly] NativeArray<int> _mapRows;
// 		[ReadOnly] NativeArray<int> _mapCols;
//
// 		WeightedRandom<int> Rand { get; }
//
// 		public void Execute(int index) {
// 			var rand    = Random.CreateFromIndex(1);
// 			var nar     = new Random();
// 			var tracker = 0;
// 			
// 			for (var row = 0; row < _mapRows[0]; row++) {
// 				for (var col = 0; col < _mapCols[0]; col++) {
// 					Output[tracker] = IsBoundary(_mapRows[0], _mapCols[0], row, col) ? 1 : Rand.Pop();
// 					tracker++;
// 				}
// 			}
// 		}
// 		
// 		static bool IsBoundary(int mapWidth, int mapHeight, int x, int y) =>
// 			x <= 0             ||
// 			y <= 0             ||
// 			x == mapWidth  - 1 ||
// 			y == mapHeight - 1;
//
// 		public void Dispose() {
// 			_mapRows.Dispose();
// 			_mapCols.Dispose();
// 			Output.Dispose();
// 		}
//
// 		public FillMapJob(ref int[,] map, WeightedRandom<int> rand) {
// 			Rand = rand;
// 			_mapRows     = new NativeArray<int>(1,          Allocator.Persistent);
// 			_mapCols     = new NativeArray<int>(1,          Allocator.Persistent);
// 			Output       = new NativeArray<int>(map.Length, Allocator.Persistent);
//
// 			_mapRows[0] = map.GetLength(0);
// 			_mapCols[0] = map.GetLength(0);
// 		}
// 	}
// }