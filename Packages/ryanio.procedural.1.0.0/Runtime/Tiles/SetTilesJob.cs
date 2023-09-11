// // ProceduralGeneration
//
// using System;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Jobs;
// using UnityEngine;
//
// namespace ProceduralGeneration {
// 	[BurstCompile]
// 	internal struct SetTilesJob : IJobParallelFor, IDisposable {
// 		public NativeArray<Vector2Int> Positions;
// 		public NativeArray<int>        CriticalIndex;
//
// 		[ReadOnly] NativeArray<int> _values;
// 		[ReadOnly] NativeArray<int> _mapRows;
// 		[ReadOnly] NativeArray<int> _mapCols;
//
// 		public void Execute(int index) {
// 			var rows = _mapRows[0];
// 			var cols = _mapCols[0];
//
// 			var tracker = 0;
// 			for (var row = 0; row < rows; row++) {
// 				for (var col = 0; col < cols; col++) {
// 					var isBoundary = IsBoundary(rows, cols, row, col);
//
// 					if (isBoundary) {
// 						_values[tracker] = 1;
// 					}
//
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
// 		bool IsFilled(int tracker) {
// 			return _values[tracker] == 1;
// 		}
//
// 		public void Dispose() {
// 			_mapCols.Dispose();
// 			_mapRows.Dispose();
// 		}
//
// 		public SetTilesJob(ref int[,] map) {
// 			Positions = new NativeArray<Vector2Int>(map.Length, Allocator.Persistent);
// 			_mapRows  = new NativeArray<int>(map.GetLength(0), Allocator.Persistent);
// 			_mapCols  = new NativeArray<int>(map.GetLength(1), Allocator.Persistent);
// 			_values   = new NativeArray<int>(map.Length,       Allocator.Persistent);
//
// 			var tracker = 0;
//
// 			for (var i = 0; i < map.GetLength(0); i++) {
// 				for (var j = 0; j < map.GetLength(1); j++) {
// 					_values[tracker] = map[i, j];
// 					tracker++;
// 				}
// 			}
//
//
// 			Positions     = new NativeArray<Vector2Int>(map.Length, Allocator.Persistent);
// 			CriticalIndex = new NativeArray<int>(1, Allocator.Persistent);
// 		}
// 	}
// }