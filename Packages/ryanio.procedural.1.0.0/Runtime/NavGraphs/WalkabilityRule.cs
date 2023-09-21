using System;
using Pathfinding;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralGeneration {
	public struct AstarScanData {
		public int     Index;
		public int     XCoord;
		public int     ZCoord;
		public Vector3 Position;
	}

	public struct MapTileData : IEquatable<MapTileData> {
		public MapTileData this[int x, int z] => x == XCoord && z == ZCoord ? this : default;

		public Vector2 Position;
		public bool    IsTile;
		public int     XCoord;
		public int     ZCoord;

		public bool Equals(MapTileData other) => XCoord == other.XCoord && ZCoord == other.ZCoord;

		public override bool Equals(object obj) => obj is MapTileData other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Position, IsTile, XCoord, ZCoord);
	}

	[Serializable, Pathfinding.Util.Preserve, BurstCompile]
	public class WalkabilityRule : GridGraphRule {
		NativeParallelHashMap<int2, bool> _mapHash;

		public WalkabilityRule() {
		}

		public WalkabilityRule(ref int[,] map) {
			var cellSize   = Constants.Instance.CellSize;
			var rows       = map.GetLength(0)            * cellSize;
			var cols       = map.GetLength(1)            * cellSize;
			var hashBuffer = rows                        * cols;

			_mapHash = new NativeParallelHashMap<int2, bool>(hashBuffer, Allocator.Persistent);

			for (var i = 0; i < rows * cols; i++) {
				var row = i / cols;  
				var col = i % cols;
				_mapHash.TryAdd(new int2(row, col), map[row / cellSize, col / cellSize] != 1);
			}

			// var offsetX =
			// 	Constants.Instance.CellSize / 2f - (Constants.Instance.CellSize * rows / 2f); // divide by 2 for center
			//
			// var offsetY =
			// 	Constants.Instance.CellSize / 2f - (Constants.Instance.CellSize * cols / 2f); // divide by 2 for center
			// for (var x = 0; x < map.GetLength(0); x++) {
			// 	for (var y = 0; y < map.GetLength(1); y++) {
			// 		var newData = new MapTileData {
			// 			Position = new Vector2(
			// 				Constants.Instance.CellSize * x + offsetX,
			// 				Constants.Instance.CellSize * y + offsetY),
			// 			IsTile = map[x, y] == 1,
			// 			XCoord = x,
			// 			ZCoord = y
			// 		};
			// 	}
			// }
		}

		public override void Register(GridGraphRules rules) {
			rules.AddJobSystemPass(Pass.AfterConnections,
				ctx => {
					var walkabilityJob = new WalkabilityJob {
						Bounds = ctx.data.bounds,
						WalkableNodes = ctx.data.nodeWalkable,
						NodeNormals = ctx.data.nodeNormals,
						AstarNodePositions = ctx.data.nodePositions,
						MapHash = _mapHash,
						CellSize = Constants.Instance.CellSize,
						ScanData = new NativeArray<AstarScanData>(ctx.data.nodePositions.Length, Allocator.Persistent),
					};

					var handle = walkabilityJob.Schedule(ctx.tracker.AllWritesDependency);
					handle.Complete();
				});
		}

		[BurstCompile(CompileSynchronously = true)]
		public struct WalkabilityJob : IJob, INodeModifier {
			public IntBounds                         Bounds;
			public NativeArray<float4>               NodeNormals;
			public NativeArray<Vector3>              AstarNodePositions;
			public NativeArray<bool>                 WalkableNodes;
			public NativeArray<AstarScanData>        ScanData;
			public NativeParallelHashMap<int2, bool> MapHash;
			public int                               CellSize;

			public void Execute() {
				ForEachNode(Bounds, NodeNormals, ref this);

				var sqSize  = CellSize * CellSize;
				var tracker = 0;

				for (var i = 0; i < MapHash.Count(); i++) {
					var currentKey = new int2(ScanData[i].XCoord, ScanData[i].ZCoord);
					var hasValue   = MapHash.TryGetValue(currentKey, out var value);

					while (tracker < sqSize) {
						if (hasValue) {
							WalkableNodes[i] &= value;
						}

						tracker++;
					}

					tracker = 0;
				}
			}

			public void ModifyNode(int dataIndex, int dataX, int dataLayer, int dataZ) {
				ScanData[dataIndex] = new AstarScanData {
					Index    = dataIndex,
					XCoord   = dataX,
					ZCoord   = dataZ,
					Position = AstarNodePositions[dataIndex]
				};

				//WalkableNodes[dataIndex] &= IsWalkable[dataIndex];
			}
		}
	}
}