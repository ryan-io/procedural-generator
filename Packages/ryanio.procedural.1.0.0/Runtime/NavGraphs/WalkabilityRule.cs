using System;
using Cathei.LinqGen;
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
		NativeList<MapTileData> _nodeData;

		public WalkabilityRule() {
		}

		public WalkabilityRule(ref int[,] map) {
			var rows = map.GetLength(0);
			var cols = map.GetLength(1);

			_nodeData = new NativeList<MapTileData>(rows * cols, Allocator.Persistent);

			var offsetX =
				Constants.Instance.CellSize / 2f - (Constants.Instance.CellSize * rows / 2f); // divide by 2 for center

			var offsetY =
				Constants.Instance.CellSize / 2f - (Constants.Instance.CellSize * cols / 2f); // divide by 2 for center

			for (var x = 0; x < map.GetLength(0); x++) {
				for (var y = 0; y < map.GetLength(1); y++) {
					var newData = new MapTileData {
						Position = new Vector2(
							Constants.Instance.CellSize * x + offsetX,
							Constants.Instance.CellSize * y + offsetY),
						IsTile = map[x, y] == 1,
						XCoord = x,
						ZCoord = y
					};

					_nodeData.AddNoResize(newData);
				}
			}
		}

		/*
		// Perform a single raycast using RaycastCommand and wait for it to complete
		// Setup the command and result buffers
		var results = new NativeArray<RaycastHit>(1, Allocator.Temp);

		var commands = new NativeArray<RaycastCommand>(1, Allocator.Temp);

		// Set the data of the first command
		Vector3 origin = Vector3.forward * -10;

		Vector3 direction = Vector3.forward;

		commands[0] = new RaycastCommand(origin, direction);

		// Schedule the batch of raycasts
		JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default(JobHandle));

		// Wait for the batch processing job to complete
		handle.Complete();

		// Copy the result. If batchedHit.collider is null there was no hit
		RaycastHit batchedHit = results[0];

		// Dispose the buffers
		results.Dispose();
		commands.Dispose();
		*/
		public override void Register(GridGraphRules rules) {
			rules.AddJobSystemPass(Pass.AfterConnections,
				ctx => {
					var walkabilityJob = new WalkabilityJob {
						Bounds = ctx.data.bounds,
						WalkableNodes = ctx.data.nodeWalkable,
						NodeNormals = ctx.data.nodeNormals,
						AstarNodePositions = ctx.data.nodePositions,
						MapTileData = _nodeData,
						ScanData = new NativeArray<AstarScanData>(ctx.data.nodePositions.Length, Allocator.Persistent),
						IsWalkable = new NativeArray<bool>(ctx.data.nodePositions.Length, Allocator.Persistent),
					};

					var handle = walkabilityJob.Schedule(ctx.tracker.AllWritesDependency);
					handle.Complete();
				});
		}

		public struct MapTilePredicate : IStructFunction<MapTileData, bool> {
			public bool Invoke(AstarScanData arg1, MapTileData arg2)
				=> arg1.XCoord == arg2.XCoord && arg1.ZCoord == arg2.ZCoord;

			public bool Invoke(MapTileData arg) => throw new NotImplementedException();
		}

		[BurstCompile(CompileSynchronously = true)]
		public struct WalkabilityJob : IJob, INodeModifier {
			public IntBounds                  Bounds;
			public NativeArray<float4>        NodeNormals;
			public NativeArray<Vector3>       AstarNodePositions;
			public NativeArray<bool>          WalkableNodes;
			public NativeList<MapTileData>    MapTileData;
			public NativeArray<bool>          IsWalkable;
			public NativeArray<AstarScanData> ScanData;

			
			public void Execute() {
				ForEachNode(Bounds, NodeNormals, ref this);

				float limit = Constants.Instance.CellSize / 2f;

				for (var i = 0; i < MapTileData.Length; i++) {
					var astarNode = ScanData[i];
					var node      = MapTileData.Gen().Where(new MapTilePredicate()).First();
				}

				// for (var i = 0; i < AstarNodePositions.Length; i++) {
				// 	var nodePosition = AstarNodePositions[i];
				//
				// 	foreach (var node in MapTileData) {
				// 		var xComp = math.abs(node.Position.x - nodePosition.x);
				// 		var yComp = math.abs(node.Position.y - nodePosition.y);
				//
				// 		if (xComp <= limit && yComp <= limit) {
				// 			IsWalkable[i] = !node.IsTile;
				// 			break;
				// 		}
				// 	}
				// }

				for (var i = 0; i < WalkableNodes.Length; i++) {
					WalkableNodes[i] &= IsWalkable[i];
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