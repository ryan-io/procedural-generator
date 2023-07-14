using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	// if (_model.DrawGizmosIfTileExists) {
	// 		for (var i = 0; i < _cachedDto.TileDataCollection.Count; i++) {
	// 			var cachedTileData = _cachedDto.TileDataCollection[i];
	// 			var pos            = new Vector3Int(cachedTileData.Location.x, cachedTileData.Location.y, 0);
	// 			var tile           = _cachedDto.GroundTilemap.GetTile(pos);
	// 	
	// 			if (!tile)
	// 				continue;
	// 	
	// 			var worldPos       = _cachedDto.GroundTilemap.CellToWorld(pos);
	// 			var tileCenterPost = new Vector3(worldPos.x + 0.5f, worldPos.y + 0.5f, 0f);
	// 	
	// 			if (cachedTileData.IsMapBoundary || cachedTileData.IsLocalBoundary)
	// 				DebugExtension.DrawCircle(tileCenterPost, _normal, Color.magenta, .25f);
	// 			else
	// 				DebugExtension.DrawCircle(tileCenterPost, _normal, Color.black, .25f);
	// 		}
	// 	}

	public interface IProceduralTileSolver {
	}

	public class ProceduralTileSolver : Singleton<ProceduralTileSolver, IProceduralTileSolver>,
	 //                                    ICreation, IValidate, IProgress {
		// UnityLogging Logger { get; set; }
	 //
		// ProceduralExitHandler _exitHandler;
	 //
		// TileTypeSolver _tileTypeSolver;

		// bool ShouldFix => MonoModel != null && MonoModel.ProceduralMapSolver == null;
		//
		// [field: SerializeField]
		// [field: HideLabel]
		// public ProceduralTileSolverMonobehaviorModel MonoModel { get; private set; }
		//
		// public ProceduralTileSolverModel Model { get; private set; }

		// public UniTask Init(CancellationToken token) {
		// 	Logger = new UnityLogging(gameObject.name);
		// 	Model = new ProceduralTileSolverModel {
		// 		TileHashset = new TileHashset()
		// 	};
		//
		// 	var outLineWeightedRandom = new WeightedRandom<int> {
		// 		{ 0, 75 },
		// 		{ 1, 25 }
		// 	};
		//
		// 	Model.TileWeightDictionary = new TileWeightDictionary {
		// 		{ "Outlines", outLineWeightedRandom }
		// 	};
		//
		// 	return new UniTask();
		// }

		// public UniTask Enable(CancellationToken token) {
		// 	var mapConfig = MonoModel.ProceduralMapSolver.MonoModel.ProceduralConfig;
		// 	var tileSolverModel = new TileSolverModel(
		// 		MonoModel, Model, mapConfig.Width, mapConfig.Height, ProceduralConfig.CELL_SIZE);
		// 	_tileTypeSolver = new SetAllTilesSyncTileTypeSolver(tileSolverModel);
		// 	return new UniTask();
		// }

		// public UniTask Begin(CancellationToken token) {
		// 	var mapConfig = MonoModel.ProceduralMapSolver.MonoModel.ProceduralConfig;
		// 	var grid      = MonoModel.TileMapGameObjects.Grid;
		//
		// 	if (!grid) {
		// 		Logger.Error("Please assign a grid object to the tile solver.");
		// 		return UniTask.FromCanceled(token);
		// 	}
		//
		// 	grid.transform.position = new Vector3(
		// 		-mapConfig.Width  / 2f + mapConfig.BorderSize / 2f,
		// 		-mapConfig.Height / 2f + mapConfig.BorderSize / 2f,
		// 		0f);
		//
		// 	return new UniTask();
		// }

		public UniTask End(CancellationToken token) => new();

		public UniTask Dispose(CancellationToken token) {
			// foreach (var tilemap in MonoModel.TileMapGameObjects.TileMapTable)
			// 	tilemap.Value.ClearAllTiles();
			//
			// return new UniTask();
		}

		public UniTask Progress_PopulatingMap(CancellationToken token) => new();

		public UniTask Progress_SmoothingMap(CancellationToken token) => new();

		public UniTask Progress_CreatingBoundary(CancellationToken token) => new();

		public UniTask Progress_RemovingRegions(CancellationToken token) => new();

		public UniTask Progress_CompilingData(CancellationToken token) => new();

		public async UniTask Progress_PreparingAndSettingTiles(CancellationToken token)
			=> await _tileTypeSolver.SetTiles(MonoModel.ProceduralMapSolver.Model.Map, token);

		public UniTask Progress_GeneratingMesh(CancellationToken token) => new();

		public UniTask Progress_CalculatingPathfinding(
			PathfindingProgressData progressData, CancellationToken token) => new();

		// void IValidate.ValidateShouldQuit() {
		// 	_exitHandler = new ProceduralExitHandler();
		//
		// 	var statements = new HashSet<Func<bool>> {
		// 		() => !Application.isPlaying,
		// 		() => MonoModel                                     == null,
		// 		() => Model                                         == null,
		// 		() => MonoModel.ProceduralTilePlacementConfig       == null,
		// 		() => MonoModel.TileMapGameObjects.Grid             == null,
		// 		() => MonoModel.TileMapGameObjects.OutlineTileTable == null,
		// 		() => MonoModel.TileMapGameObjects.TileMapTable     == null
		// 	};
		//
		// 	_exitHandler.DetermineQuit(statements.ToArray());
		// }
		//
		// [Button]
		// [ShowIf("@ShouldFix")]
		// void Fix() {
		// 	MonoModel.ProceduralMapSolver = gameObject.FixComponent<ProceduralMapSolver>();
		// }
	}
}