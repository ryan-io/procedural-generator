using System;
using System.Collections.Generic;
using Pathfinding;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * 	// var extends = new Vector3(CellSize / 2f, CellSize / 2f, CellSize / 2f);
						//
						// for (var i = 0; i < _collisions.Value.Length; i++) {
						// 	if (_collisions.Value[i])
						// 		_collisions.Value[i] = null;
						// }
						
						// var size    = Physics.OverlapBoxNonAlloc(worldPosGround, extends, _collisions.Value);
 */
namespace Engine.Procedural {
	[Serializable]
	public class WalkabilityRule : GridGraphRule {
		[SerializeField] public Tilemap BoundaryTilemap;
		[SerializeField] public Tilemap GroundTilemap;
		[SerializeField] public float   CellSize;
		Lazy<Collider[]>                _collisions = new(() => new Collider[10]);

		public WalkabilityRule() {
		}

		public WalkabilityRule(Tilemap boundaryTilemap, Tilemap groundTilemap, float cellSize) {
			BoundaryTilemap = boundaryTilemap;
			GroundTilemap   = groundTilemap;
			CellSize        = cellSize;
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
					var positions    = ctx.data.nodePositions.ToArray();
					var hasTilesList = new List<bool>();

					foreach (var position in positions) {
						bool hasTile;

						if (!BoundaryTilemap || !GroundTilemap)
							continue;

						var worldPosBoundary = BoundaryTilemap.WorldToCell(position);
						var worldPosGround   = GroundTilemap.WorldToCell(position);
						var hasTileGround    = GroundTilemap.HasTile(worldPosGround);
						var hasTileBoundary  = BoundaryTilemap.HasTile(worldPosBoundary);

						var results      = new NativeArray<RaycastHit>(1, Allocator.TempJob);
						var commands     = new NativeArray<RaycastCommand>(1, Allocator.TempJob);
						var direction    = -Vector3.forward;
						var positionCast = new Vector3(position.x, position.y, -0.5f);
						commands[0] = new RaycastCommand(positionCast, direction);
						var handlePhys = RaycastCommand.ScheduleBatch(commands, results, 1);
						handlePhys.Complete();
						var batchedHit = results[0];

						if (!hasTileGround || hasTileBoundary) {
							hasTile = false;
						}
						else {
							if (batchedHit.collider != null && !batchedHit.collider.isTrigger) {
								Debug.Log("Collider found!");
								hasTile = false;
							}
							else {
								hasTile = true;
							}
						}

						hasTilesList.Add(hasTile);
						results.Dispose();
						commands.Dispose();
					}

					//var test = ctx.tracker.NewNativeArray<bool>();

					var hasTileNativeArray = new NativeArray<bool>(hasTilesList.ToArray(), Allocator
					   .Persistent);

					var walkabilityJob = new WalkabilityJobData {
						Bounds        = ctx.data.bounds,
						WalkableNodes = ctx.data.nodeWalkable,
						NodeNormals   = ctx.data.nodeNormals,
						HasTiles      = hasTileNativeArray
					};

					var handle = walkabilityJob.Schedule(ctx.tracker.AllWritesDependency);
					handle.Complete();
					hasTileNativeArray.Dispose();
				});
		}

		[BurstCompile]
		struct WalkabilityJobData : IJob, INodeModifier {
			public IntBounds           Bounds;
			public NativeArray<float4> NodeNormals;

			[ReadOnly] public NativeArray<bool> HasTiles;

			public NativeArray<bool> WalkableNodes;

			public void Execute() {
				ForEachNode(Bounds, NodeNormals, ref this);
			}

			public void ModifyNode(int dataIndex, int dataX, int dataLayer, int dataZ) {
				WalkableNodes[dataIndex] &= HasTiles[dataIndex];
			}
		}
	}
}