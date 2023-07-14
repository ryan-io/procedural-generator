using System.Threading;
using Cysharp.Threading.Tasks;
using UnityBCL;
using StateMachine;

namespace Engine.Procedural {
	public class CreationFlow {
		UnityLogging Logger { get; }

		readonly StateMachine<CreationState> _creationSm;
		readonly FlowDependency              _flowDependency;
		readonly StateMachine<ProgressState> _progressSm;
		readonly StateMachine<RuntimeState>  _runtimeSm;

		public CreationFlow(FlowDependency flowDependency) {
			Logger = new UnityLogging(flowDependency.FlowComponents.ProceduralMapStateMachine.gameObject.name);
			_flowDependency = flowDependency;
			_creationSm     = _flowDependency.FlowComponents.ProceduralMapStateMachine.CreationSm;
			_progressSm     = _flowDependency.FlowComponents.ProceduralMapStateMachine.ProgressSm;
			_runtimeSm      = _flowDependency.FlowComponents.ProceduralMapStateMachine.RuntimeSm;
		}

		public async UniTask HandleFlow(EventStateChange<CreationState> e, float timeElapsed, CancellationToken token) {
			switch (e.NewState) {
				case CreationState.Cleaning:
					if (_runtimeSm.CurrentState == RuntimeState.DoNotGenerate) {
						Logger.Warning(
							"Runtime state is set to 'DoNotGenerate'. Generation process has not been run.");
						_creationSm.ChangeState(CreationState.DidNotGenerate);
						break;
					}

					await ResetDependencies(token);
					_creationSm.ChangeState(CreationState.Initializing);
					break;
				case CreationState.Initializing:
					await InitializeDependencies(token);
					_creationSm.ChangeState(CreationState.Enabling);
					break;
				case CreationState.Enabling:
					await EnableDependencies(token);
					_creationSm.ChangeState(CreationState.Starting);
					break;
				case CreationState.Starting:
					await StartDependencies(token);
					_creationSm.ChangeState(CreationState.InProgress);
					break;
				case CreationState.InProgress:
					_progressSm.ChangeState(ProgressState.Starting);
					// typically ProgressState flow will be running during this
					//ProceduralMapStateMachine.Global.CreationSm.ChangeState(CreationState.Ending);
					break;
				case CreationState.Ending:
					await EndDependencies(token);
					_creationSm.ChangeState(CreationState.Complete);
					break;
				case CreationState.Complete:
					_flowDependency.FlowComponents.Events.OnCompleteObservable.Signal();
					_flowDependency.FlowComponents.Events.Hook.OnComplete.Invoke();
					var onCompleteData = new MapGenerationData(_flowDependency.FlowComponents.ProceduralMapSolver
					                                                          .MonoModel.ProceduralConfig
					                                                          .GetDimensionsVector);
					_flowDependency.FlowComponents.Events.Hook.OnCompleteWithData.Invoke(onCompleteData);
					break;
				case CreationState.Cancelling:
					break;
				case CreationState.Disposing:
					await ResetDependencies(token);
					break;
			}
		}

		async UniTask ResetDependencies(CancellationToken token) {
			var components = _flowDependency.GetComponentsAsCreation;
			foreach (var component in components)
				await component.Dispose(token);
		}

		async UniTask InitializeDependencies(CancellationToken token) {
			var components = _flowDependency.GetComponentsAsCreation;

			foreach (var component in components)
				await component.Init(token);
		}

		async UniTask EnableDependencies(CancellationToken token) {
			var components = _flowDependency.GetComponentsAsCreation;

			foreach (var component in components)
				await component.Enable(token);
		}

		async UniTask StartDependencies(CancellationToken token) {
			var components = _flowDependency.GetComponentsAsCreation;

			foreach (var component in components)
				await component.Begin(token);
		}

		async UniTask EndDependencies(CancellationToken token) {
			var components = _flowDependency.GetComponentsAsCreation;

			foreach (var component in components)
				await component.End(token);
		}
	}
}