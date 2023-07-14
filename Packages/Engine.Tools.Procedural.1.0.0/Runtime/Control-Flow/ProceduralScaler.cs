// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Sirenix.OdinInspector;
// using Source;
// using Source.Events;
// using Unity.Mathematics;
// using UnityBCL;
// using UnityEngine;
//
// namespace Engine.Procedural {
// 	[Serializable]
// 	public class ProceduralScalerMonobehaviorModel {
// 		[field: SerializeField]
// 		[field: Title("Required Monobehaviors")]
// 		[field: Required]
// 		public ProceduralMapSolver ProceduralMapSolver { get; set; }
//
// 		[field: SerializeField]
// 		[field: Required]
// 		public ProceduralTileSolver ProceduralTileSolver { get; set; }
// 	}
//
// 	// public class ProceduralScaler : Singleton<ProceduralScaler, ProceduralScaler>,
// 	//                                 IEngineEventListener<ProgressState>, IValidate {
// 		// [SerializeField] [HideLabel] ProceduralScalerMonobehaviorModel _monoModel;
// 		//
// 		// bool ShouldFix => _monoModel != null && _monoModel.GetPropertyValues<object>().Any(x => x == null);
// 		//
// 		// void IValidate.ValidateShouldQuit() {
// 		// 	var exitHandler = new ProceduralExitHandler();
// 		//
// 		// 	var statements = new HashSet<Func<bool>> {
// 		// 		() => _monoModel                      == null,
// 		// 		() => _monoModel.ProceduralMapSolver  == null,
// 		// 		() => _monoModel.ProceduralTileSolver == null
// 		// 	};
// 		//
// 		// 	exitHandler.DetermineQuit(statements.ToArray());
// 		// }
//
// 		// public void OnEventHeard(ProgressState e) {
// 		// 	if (e != ProgressState.ScalingGrid)
// 		// 		return;
// 		//
// 		// 	SetGridOrigin();
// 		// 	SetGridScale();
// 		// }
//
// 		// void SetGridOrigin() {
// 		// 	var mapConfig        = _monoModel.ProceduralMapSolver.MonoModel.ProceduralConfig;
// 		// 	var tileSceneObjects = _monoModel.ProceduralTileSolver.MonoModel.TileMapGameObjects;
// 		//
// 		// 	tileSceneObjects.Grid.gameObject.transform.position = ProcessNewPosition(mapConfig.GetDimensionsInteger);
// 		// }
//
// 		// static Vector3 ProcessNewPosition(int2 mapSize) => new(
// 		// 	Mathf.CeilToInt(-mapSize.x  / 2f),
// 		// 	Mathf.FloorToInt(-mapSize.y / 2f),
// 		// 	0);
// 		//
// 		// void SetGridScale() {
// 		// 	var mapConfig        = _monoModel.ProceduralMapSolver.MonoModel.ProceduralConfig;
// 		// 	var tileSceneObjects = _monoModel.ProceduralTileSolver.MonoModel.TileMapGameObjects;
// 		//
// 		// 	tileSceneObjects.Grid.gameObject.transform.localScale = ProcessNewScale(ProceduralConfig.CELL_SIZE);
// 		// }
//
// 		//static Vector3 ProcessNewScale(int cellSize) => new(cellSize, cellSize, cellSize);
//
// 		// void OnEnable() {
// 		// 	this.StartListeningToEvents();
// 		// }
// 		//
// 		// void OnDisable() {
// 		// 	this.StopListeningToEvents();
// 		// }
// 		//
// 		// [Button]
// 		// [ShowIf("@ShouldFix")]
// 		// void Fix() {
// 		// 	_monoModel.ProceduralMapSolver  = gameObject.FixComponent<ProceduralMapSolver>();
// 		// 	_monoModel.ProceduralTileSolver = gameObject.FixComponent<ProceduralTileSolver>();
// // 		// }
// // 	}
// // }