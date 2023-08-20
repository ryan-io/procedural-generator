// Engine.Procedural

using System.Threading;
using Cysharp.Threading.Tasks;
using Pathfinding;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	public class NodeSerializationSolver : AsyncUnitOfWork {
		GraphScanner      GraphScanner               { get; }
		ISeed             Seed                       { get; }
		WalkabilityRule   WalkabilityRule            { get; }
		TileMapDictionary TileMapDictionary          { get; }
		ColliderType      CollisionType              { get; }
		LayerMask         ObstacleLayerMask          { get; }
		LayerMask         HeightTestLayerMask        { get; }
		string            Name                       { get; }
		string            SerializedDataSaveLocation { get; }
		float             CollisionDetectionDiameter { get; }
		float             CollisionDetectionHeight   { get; }

		protected override async UniTask TaskLogic(CancellationToken token) {
			DeserializeAstarNodeData();

			var status = SetInitialTilemapStatus(
				TileMapDictionary[TileMapType.Ground],
				TileMapDictionary[TileMapType.Boundary]);

			var gridGraph = await SetupGridGraphRules(token);

			gridGraph.collision.use2D                  = false;
			gridGraph.collision.heightCheck            = true;
			gridGraph.collision.unwalkableWhenNoGround = false;
			gridGraph.collision.type                   = CollisionType;
			gridGraph.collision.heightMask             = HeightTestLayerMask;
			gridGraph.collision.diameter               = CollisionDetectionDiameter;
			gridGraph.collision.height                 = CollisionDetectionHeight;
			gridGraph.collision.mask                   = ObstacleLayerMask;

			TileMapDictionary[TileMapType.Ground].enabled   = status.groundStatus;
			TileMapDictionary[TileMapType.Boundary].enabled = status.boundaryStatus;
		}

		async UniTask<GridGraph> SetupGridGraphRules(CancellationToken token) {
			var gridGraph = AstarPath.active.data.gridGraph;
			new GridGraphRuleRemover().Remove(gridGraph);

			gridGraph.rules.AddRule(WalkabilityRule);

			var args = new GraphScanner.Args(gridGraph, null, true);

			//TODO: This method will not currently work
			await GraphScanner.FireTask(args, token);

			return gridGraph;
		}

		void DeserializeAstarNodeData() {
			var info = Seed.GetSeedInfo();
			// var job = new ProceduralSerializer.Job(
			// 	SerializedDataSaveLocation, info.LastSeed, info.LastIteration, Name);
			//
			// new ProceduralSerializer(StopWatch).DeserializeAstarGraph(job);
		}

		(bool groundStatus, bool boundaryStatus) SetInitialTilemapStatus(
			Tilemap groundTilemap, Tilemap boundaryTilemap) {
			var groundTilemapStatus   = groundTilemap.enabled;
			var boundaryTilemapStatus = boundaryTilemap.enabled;

			groundTilemap.enabled   = true;
			boundaryTilemap.enabled = true;

			return (groundTilemapStatus, boundaryTilemapStatus);
		}

		public NodeSerializationSolver(ProceduralConfig config, ISeed seed, TileMapDictionary dictionary) {
			GraphScanner = new GraphScanner();
			WalkabilityRule = new WalkabilityRule(
				dictionary[TileMapType.Boundary],
				dictionary[TileMapType.Ground],
				Constants.CELL_SIZE);

			Seed                       = seed;
			TileMapDictionary          = dictionary;
			Name                       = config.Name;
			CollisionType              = config.NavGraphCollisionType;
			ObstacleLayerMask          = config.ObstacleLayerMask;
			HeightTestLayerMask        = config.NavGraphHeightTestLayerMask;
			CollisionDetectionDiameter = config.NavGraphCollisionDetectionDiameter;
			CollisionDetectionHeight   = config.NavGraphCollisionDetectionHeight;
			SerializedDataSaveLocation = Constants.SERIALIZED_DATA_FOLDER;
		}
	}
}