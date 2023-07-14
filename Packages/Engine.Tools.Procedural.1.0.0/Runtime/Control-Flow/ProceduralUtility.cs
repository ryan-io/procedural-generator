// using System;
// using System.Linq;
// using System.Threading;
// using Cysharp.Threading.Tasks;
// using Sirenix.OdinInspector;
// using Source;
// using Source.Events;
// using StateMachine;
// using Unity.Mathematics;
// using UnityBCL;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Profiling;
//
// namespace Engine.Procedural {
// 	// [Serializable]
// 	// public class ProceduralMapUtilityMonobehaviorModel {
// 	// 	[field: SerializeField]
// 	// 	[field: Required]
// 	// 	public ProceduralMapStateMachine ProceduralMapStateMachine { get; set; }
// 	//
// 	// 	[field: SerializeField]
// 	// 	[field: Required]
// 	// 	public ProceduralMapSolver ProceduralMapSolver { get; set; }
// 	//
// 	// 	[field: SerializeField]
// 	// 	[field: Required]
// 	// 	public ProceduralTileSolver ProceduralTileSolver { get; set; }
// 	// }
//
// 	public class ProceduralUtility : Singleton<ProceduralUtility, ProceduralUtility>,
// 	                                 ICreation,
// 	                                 IEngineEventListener<EventStateChange<ProgressState>>,
// 	                                 IEngineEventListener<GridSetEvent> {
// 		// [Title("Required Monobehaviors")] [SerializeField] [HideLabel]
// 		// ProceduralMapUtilityMonobehaviorModel _monoModel;
// 		//
// 		//
// 		//
// 		// bool ShouldFix => _monoModel != null && _monoModel.GetPropertyValues<object>().Any(x => x == null);
//
// 		// public UniTask Init(CancellationToken token) {
// 		// 	this.StartListeningToEvents<GridSetEvent>();
// 		// 	this.StartListeningToEvents<EventStateChange<ProgressState>>();
// 		// 	return new UniTask();
// 		// }
//
// 		//public UniTask Enable(CancellationToken token) => new();
//
// 		// public UniTask Begin(CancellationToken token) => new();
// 		//
// 		// public UniTask End(CancellationToken token) {
// 		// 	this.StopListeningToEvents<GridSetEvent>();
// 		// 	this.StopListeningToEvents<EventStateChange<ProgressState>>();
// 		// 	return new UniTask();
// 		// }
//
// 		//public UniTask Dispose(CancellationToken token) => new();
//
// 		// public static void LogAllocatedMemory() {
// 		// 	var log = new UnityLogging();
// 		// 	log.Msg(
// 		// 		$"Memory usage: {TotalMemoryAllocated} GB"
// 		// 		, size: 15, italic: true, bold: true, ctx: $"{Strings.ProcGen} Heap Allocation");
// 		// }
// 		//
// 		// public static float DetermineTotalTime(float milliseconds, out string unit) {
// 		// 	float totalTime;
// 		//
// 		// 	if (milliseconds >= 1000) {
// 		// 		totalTime = milliseconds / 1000f;
// 		// 		unit      = "seconds";
// 		// 	}
// 		//
// 		// 	else {
// 		// 		totalTime = milliseconds;
// 		// 		unit      = "mSeconds";
// 		// 	}
// 		//
// 		// 	return totalTime;
// 		// }
//
//
//
// #endif
// 		//
// 		// public void OnEventHeard(GridSetEvent e) {
// 		// 	/*
// 		// 	 * 	MapGridDirector.SetGridOrigin(_tileSceneObjects, _generatorConfig.ConfigDimensions);
// 		// 	MapGridDirector.SetGridScale(_tileSceneObjects, _generatorConfig.cellSize);
// 		// 	 */
// 		// 	GridUtil.SetGridOrigin(e.SceneObjects, e.MapSize);
// 		// 	GridUtil.SetGridScale(e.SceneObjects, e.Cellsize);
// 		// }
// 		//
// 		// public void OnEventHeard(EventStateChange<ProgressState> e) {
// 		// 	if (e.NewState == ProgressState.Disposing) RemoveLabels();
// 		// }
// 		//
// 		// [Button]
// 		// [ShowIf("@ShouldFix")]
// 		// void Fix() {
// 		// 	_monoModel.ProceduralMapSolver       = gameObject.FixComponent<ProceduralMapSolver>();
// 		// 	_monoModel.ProceduralTileSolver      = gameObject.FixComponent<ProceduralTileSolver>();
// 		// 	_monoModel.ProceduralMapStateMachine = gameObject.FixComponent<ProceduralMapStateMachine>();
// 		// }
// 	}
// }