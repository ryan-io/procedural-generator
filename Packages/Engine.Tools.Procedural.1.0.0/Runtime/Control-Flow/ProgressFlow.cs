using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Events;
using StateMachine;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public class ProgressFlow : IEngineEventListener<MeshGenerationData> {
		UnityLogging Logger { get; }
		
		readonly StateMachine<CreationState> _creationSm;
		readonly FlowDependency              _flowDependency;
		readonly StateMachine<ProgressState> _progressSm;
		readonly StateMachine<RuntimeState>  _runtimeSm;

		public ProgressFlow(FlowDependency flowDependency) {
			Logger          = new UnityLogging(flowDependency.FlowComponents.ProceduralMapSolver.gameObject.name);
			_flowDependency = flowDependency;
			_creationSm     = _flowDependency.FlowComponents.ProceduralMapStateMachine.CreationSm;
			_progressSm     = _flowDependency.FlowComponents.ProceduralMapStateMachine.ProgressSm;
			_runtimeSm      = _flowDependency.FlowComponents.ProceduralMapStateMachine.RuntimeSm;
		}

		public async UniTask HandleFlow(EventStateChange<ProgressState> e, float elapsedTime, CancellationToken token) {
			switch (e.NewState) {
				// case ProgressState.Starting:
				// 	HandleStarting();
				// 	_flowDependency.FlowComponents.Events.Hook.OnStartComplete?.Invoke();
				// 	break;
				
				// *** COME BACK TO THIS LATER -> THERE IS A CHANCE THIS BECOMES INVOKED
				case ProgressState.CompilingData:
					await _flowDependency.FlowComponents.ProceduralMapSolver.Progress_CompilingData(token);
					await _flowDependency.FlowComponents.ProceduralPathfindingSolver.Progress_CompilingData(token);
					_flowDependency.FlowComponents.Events.Hook.OnCompilingData?.Invoke();
					this.StopListeningToEvents();
					_progressSm.ChangeState(ProgressState.Complete);
					break;
				// *** COME BACK TO THIS LATER -> THERE IS A CHANCE THIS BECOMES INVOKED
				
				// case ProgressState.PopulatingMap:
				// 	await _flowDependency.FlowComponents.ProceduralMapSolver.Progress_PopulatingMap(token);
				// 	_progressSm.ChangeState(ProgressState.SmoothingMap);
				// 	_flowDependency.FlowComponents.Events.Hook.OnPopulatingMap?.Invoke();
				// 	break;
				// case ProgressState.SmoothingMap:
				// 	await _flowDependency.FlowComponents.ProceduralMapSolver.Progress_SmoothingMap(token);
				// 	_progressSm.ChangeState(ProgressState.RemovingRegions);
				// 	_flowDependency.FlowComponents.Events.Hook.OnSmoothingMap?.Invoke();
				// 	break;
				// case ProgressState.RemovingRegions:
				// 	await _flowDependency.FlowComponents.ProceduralMapSolver.Progress_RemovingRegions(token);
				// 	_progressSm.ChangeState(ProgressState.CreatingBoundary);
				// 	_flowDependency.FlowComponents.Events.Hook.OnRemovingRegions?.Invoke();
				// 	break;
				//case ProgressState.CreatingBoundary:
					//await _flowDependency.FlowComponents.ProceduralMapSolver.Progress_CreatingBoundary(token);
					// _progressSm.ChangeState(ProgressState.ScalingGrid);
					// _flowDependency.FlowComponents.Events.Hook.OnCreatingBoundary?.Invoke();
					//break;
				// case ProgressState.ScalingGrid:
				// 	InvokeGridSetEvent();
				// 	_progressSm.ChangeState(ProgressState.PreparingAndSettingTiles);
				// 	_flowDependency.FlowComponents.Events.Hook.OnScalingGrid?.Invoke();
				// 	break;
				// case ProgressState.PreparingAndSettingTiles:
				// 	//await _flowDependency.FlowComponents.ProceduralTileSolver.Progress_PreparingAndSettingTiles(token);
				// 	//await _flowDependency.FlowComponents.ProceduralMeshSolver.Progress_PreparingAndSettingTiles(token);
				// 	//_progressSm.ChangeState(ProgressState.GeneratingMesh);
				// 	_flowDependency.FlowComponents.Events.Hook.OnPreparingAndSettingTiles?.Invoke();
				// 	break;
				// case ProgressState.GeneratingMesh:
				// 	//await _flowDependency.FlowComponents.ProceduralMeshSolver.Progress_GeneratingMesh(token);
				// 	//_flowDependency.FlowComponents.Events.Hook.OnGeneratingMesh?.Invoke();
				// 	break;
				// case ProgressState.CalculatingPathfinding:
				// 	//_flowDependency.FlowComponents.Events.Hook.OnCalculatingPathfinding?.Invoke();
				// 	//var progressData = CreateProgressData();
				// 	// await _flowDependency.FlowComponents.ProceduralPathfindingSolver.Progress_CalculatingPathfinding(
				// 	// 	progressData, token);
				// 	//_progressSm.ChangeState(ProgressState.CompilingData);
				// 	break;
				case ProgressState.Complete:
					_flowDependency.FlowComponents.Events.OnCompleteObservable.Signal();
					await Resources.UnloadUnusedAssets();
					ProceduralUtility.LogAllocatedMemory();
					_creationSm.ChangeState(CreationState.Ending);
					break;
			}
		}

		// void InvokeGridSetEvent() {
		// 	var mapConfig = _flowDependency.FlowComponents.ProceduralMapSolver.MonoModel.ProceduralConfig;
		// 	var sceneObjs = _flowDependency.FlowComponents.ProceduralTileSolver.MonoModel.TileMapGameObjects;
		// 	var gridSetEvent = new GridSetEvent(sceneObjs, mapConfig.GetDimensionsInteger, ProceduralConfig.CELL_SIZE);
		//
		// 	var proxy = new InternalEventProxy();
		// 	proxy.TriggerEvent(gridSetEvent);
		// }

		// PathfindingProgressData CreateProgressData() {
		// 	var mapConfig = _flowDependency.FlowComponents.ProceduralMapSolver.MonoModel.ProceduralConfig;
		// 	var tileModel = _flowDependency.FlowComponents.ProceduralTileSolver.Model;
		// 	var tileMonoModel = _flowDependency.FlowComponents.ProceduralTileSolver.MonoModel;
		// 	var iteration = _flowDependency.FlowComponents.ProceduralMapSolver.LastIteration;
		//
		// 	var data = new PathfindingProgressData(
		// 		mapConfig.GetDimensionsInteger,
		// 		ProceduralConfig.CELL_SIZE,
		// 		tileMonoModel.BoundaryTilemap,
		// 		tileMonoModel.GroundTilemap,
		// 		tileModel.TileHashset,
		// 		_flowDependency.FlowComponents.ProceduralGenerator,
		// 		mapConfig.Seed,
		// 		iteration,
		// 		mapConfig.Name);
		//
		// 	return data;
		// }

		// void HandleStarting() {
		// 	this.StartListeningToEvents();
		//
		// 	if (_runtimeSm.CurrentState == RuntimeState.Generate) {
		// 		_progressSm.ChangeState(ProgressState.PopulatingMap);
		// 		Logger.Msg("Generation beginning shortly...", ctx: "Procedural Generation - Message", bold: true,
		// 			italic: true, size: 14);
		// 	}
		// 	else {
		// 		_progressSm.ChangeState(ProgressState.CompilingData);
		// 		Logger.Warning("Generator is set to 'DoNotGenerate'. Procedural generation will not be run.",
		// 			ctx: "Procedural Generation - Message");
		// 	}
		// }
		//
		// void IEngineEventListener<MeshGenerationData>.OnEventHeard(MeshGenerationData e) {
		// 	_progressSm.ChangeState(ProgressState.CalculatingPathfinding);
		// }
	}
}