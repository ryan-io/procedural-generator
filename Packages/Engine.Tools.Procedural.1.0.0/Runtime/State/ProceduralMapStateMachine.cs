using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Source.Events;
using UnityBCL;
using UnityEngine;
using StateMachine;

namespace Engine.Procedural {
	public interface IProceduralMapStateMachine {
	}

	public class ProceduralMapStateMachine : Singleton<ProceduralMapStateMachine, IProceduralMapStateMachine>,
	                                         ICreation, IValidate, IEngineEventListener<CreationState> {
		[SerializeField] [HideLabel] ProceduralMapStateMachineMonobehaviorModel _monoModel;
		public                       StateMachine<ApplicationState>             ApplicationSm { get; private set; }
		public                       StateMachine<RuntimeState>                 RuntimeSm     { get; private set; }
		public                       StateMachine<CreationState>                CreationSm    { get; private set; }
		public                       StateMachine<ProgressState>                ProgressSm    { get; private set; }

		bool ShouldFix => _monoModel != null && _monoModel.GetPropertyValues<object>().Any(x => x == null);

		//public UniTask Init(CancellationToken token) => new();

		//public UniTask Enable(CancellationToken token) => new();

		public UniTask Begin(CancellationToken token) => new();

		IEngineEvent EventProxy { get; set; } = new InternalEventProxy();

		public UniTask End(CancellationToken token) {
			ApplicationSm.DeleteSubscribers();
			CreationSm.DeleteSubscribers();
			RuntimeSm.DeleteSubscribers();
			ProgressSm.DeleteSubscribers();
			return new UniTask();
		}

		public UniTask Dispose(CancellationToken token) {
			this.StartListeningToEvents();
			return new UniTask();
		}

		// public void ValidateShouldQuit() {
		// 	var exitHandler = new ProceduralExitHandler();
		//
		// 	var statements = new HashSet<Func<bool>> {
		// 		() => _monoModel                      == null,
		// 		() => _monoModel.ProceduralGenerator == null
		// 	};
		//
		// 	exitHandler.DetermineQuit(statements.ToArray());
		// }

		public void CreateStateMachines() {
			this.StartListeningToEvents();
			var go = gameObject;
			ApplicationSm = new StateMachine<ApplicationState>(go,  true);
			CreationSm    = new StateMachine<CreationState>(go,  true);
			ProgressSm    = new StateMachine<ProgressState>(go, true);
			RuntimeSm     = new StateMachine<RuntimeState>(go, true);
		}

		public void RegisterStateMachines() {
			ApplicationSm.OnStateChange +=
				() => GenLogging.LogStateChange(
					typeof(ApplicationState), ApplicationSm.CurrentState,
					_monoModel.ProceduralGenerator.GetTimeElapsedInMilliseconds);
			CreationSm.OnStateChange +=
				() => GenLogging.LogStateChange(typeof(CreationState), CreationSm.CurrentState,
					_monoModel.ProceduralGenerator.GetTimeElapsedInMilliseconds);
			ProgressSm.OnStateChange +=
				() => GenLogging.LogStateChange(typeof(ProgressState), ProgressSm.CurrentState,
					_monoModel.ProceduralGenerator.GetTimeElapsedInMilliseconds);
			RuntimeSm.OnStateChange +=
				() => GenLogging.LogStateChange(typeof(RuntimeState), RuntimeSm.CurrentState,
					_monoModel.ProceduralGenerator.GetTimeElapsedInMilliseconds);
		}

		public void SetStateMachines() {
			var coreConfig = _monoModel.ProceduralGenerator.CoreConfiguration;
			ApplicationSm.ChangeState(coreConfig.ApplicationState);
			RuntimeSm.ChangeState(coreConfig.RuntimeState);
			ProgressSm.ChangeState(ProgressState.Pending);
			CreationSm.ChangeState(CreationState.Pending);
		}

		public void ForceAllInitialize() {
			CreateStateMachines();
			RegisterStateMachines();
			SetStateMachines();
		}

		void Awake() {
			CreateStateMachines();
		}

		void OnEnable() {
			RegisterStateMachines();
		}

		void Start() {
			SetStateMachines();
		}

		[Button]
		[ShowIf("@ShouldFix")]
		void Fix() {
			_monoModel.ProceduralGenerator = gameObject.FixComponent<ProceduralGenerator>();
		}

		public void OnEventHeard(CreationState e) {
		}
	}
}