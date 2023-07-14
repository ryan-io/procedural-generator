using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.Procedural {
	public class ProceduralEditorGenerator : Singleton<ProceduralEditorGenerator, ProceduralEditorGenerator> {
		[FormerlySerializedAs("_controller")] [Title("Required Monobehaviors")] [SerializeField] [Required]
		ProceduralGenerator Generator;

		[SerializeField] [PropertySpace(0, 10)] [Required]
		ProceduralMapStateMachine _stateMachine;

		bool ShouldFix => !Generator || !_stateMachine;

		[Button]
		[ShowIf("@ShouldFix")]
		void Fix() {
			Generator   = gameObject.FixComponent<ProceduralGenerator>();
			_stateMachine = gameObject.FixComponent<ProceduralMapStateMachine>();
		}

		[Button]
		[TabGroup("1", "Procedural Generation Control")]
		async void GenerateMap() {
			_stateMachine.ForceAllInitialize();
			Generator.ResetToken();
			Generator.Initialize();
			await Generator.BeginGeneration();
		}

		[Button(ButtonStyle.CompactBox)]
		[TabGroup("1", "Procedural Generation Control")]
		void ResetMap() {
			Generator.ResetToken();
			foreach (var component in Generator.GetFlowComponentsAsCreation)
				component.Dispose(Generator.CancellationToken);
		}

		[Button(ButtonStyle.CompactBox)]
		[TabGroup("1", "Procedural Generation Control")]
		public void CancelGeneration() => Generator.ResetToken(false);
	}
}