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
namespace ProceduralGeneration {
	[Serializable, Pathfinding.Util.Preserve]
	public class WalkabilityRule : GridGraphRule {
		Tilemap     _boundaryTilemap;
		Tilemap     _groundTilemap;
		TileHashset _tileHashset;

		public WalkabilityRule() {
		}

		public WalkabilityRule(Tilemap boundaryTilemap, Tilemap groundTilemap, TileHashset tileHashset) {
			_boundaryTilemap = boundaryTilemap;
			_groundTilemap   = groundTilemap;
			_tileHashset     = tileHashset;
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
					var size         = ctx.graph.nodeSize;

					const int allocationSize = 4;

					foreach (var position in positions) {
						bool hasTile;

						if (!_boundaryTilemap || !_groundTilemap)
							continue;

						var worldPosBoundary = _boundaryTilemap.WorldToCell(position);
						var worldPosGround   = _groundTilemap.WorldToCell(position);
						var hasTileGround    = _groundTilemap.HasTile(worldPosGround);
						var hasTileBoundary  = _boundaryTilemap.HasTile(worldPosBoundary);

						// RayCastHit & RayCastCommand buffers
						var results  = new NativeArray<RaycastHit>(allocationSize, Allocator.TempJob);
						var commands = new NativeArray<RaycastCommand>(allocationSize, Allocator.TempJob);

						var direction = -Vector3.forward;

						//var scaledPosition = position / (size);

						//var positionCast1 = new Vector3(scaledPosition.x,  scaledPosition.y,  -0.5f);
						// var positionCast2 = new Vector3(-scaledPosition.x, -scaledPosition.y, -0.5f);
						// var positionCast3 = new Vector3(-scaledPosition.x, scaledPosition.y,  -0.5f);
						// var positionCast4 = new Vector3(scaledPosition.x,  -scaledPosition.y, -0.5f);
						
						commands[0] = new RaycastCommand(position, direction, QueryParameters.Default);
						// commands[1] = new RaycastCommand(positionCast2,  direction, QueryParameters.Default);
						// commands[2] = new RaycastCommand(positionCast3,  direction, QueryParameters.Default);
						// commands[3] = new RaycastCommand(positionCast4,  direction, QueryParameters.Default);

						var handlePhys = RaycastCommand.ScheduleBatch(commands, results, 1);

						handlePhys.Complete();

						var batchedHit = results[0];
						
						if (!hasTileGround || hasTileBoundary) {
							hasTile = false;
						}
						else {
							// var acceptable = results.Where(hit => hit.collider && !hit.collider.isTrigger);
							// hasTile = !acceptable.Any();
							if (batchedHit.collider && !batchedHit.collider.isTrigger) {
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

					var hasTileNativeArray = new NativeArray<bool>(hasTilesList.ToArray(), Allocator.Persistent);

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