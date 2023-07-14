using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public class ProceduralGeneratorSetup : Singleton<ProceduralGeneratorSetup, ProceduralGeneratorSetup> {
		[SerializeField] [HideInInspector] bool _shouldShowRemoveButton;

		[Button(ButtonSizes.Large)]
		void SetupSceneMonobehaviors() {
#if UNITY_EDITOR
			gameObject.FixComponent<ProceduralEditorGenerator>();
			gameObject.FixComponent<GenLogging>();
#endif

			gameObject.FixComponent<MeshFilter>();
			gameObject.FixComponent<ProceduralMapSolver>();
			gameObject.FixComponent<ProceduralGenerator>();
			gameObject.FixComponent<ProceduralMeshSolver>();
			gameObject.FixComponent<ProceduralTileSolver>();
			gameObject.FixComponent<ProceduralPathfindingSolver>();
			gameObject.FixComponent<ProceduralScaler>();
			gameObject.FixComponent<ProceduralGenerationEvents>();
			gameObject.FixComponent<ProceduralMapStateMachine>();
			gameObject.FixComponent<ProceduralUtility>();
			gameObject.FixComponent<ProceduralMapSolver>();
			gameObject.FixComponent<ProceduralSceneBounds>();
			gameObject.FixComponent<ProceduralMapHandler>();

			_shouldShowRemoveButton = true;
		}

		[Button(ButtonSizes.Large)]
		[ShowIf("@_shouldShowRemoveButton")]
		void RemoveThisComponent() {
#if UNITY_EDITOR
			DestroyImmediate(this);
#endif
		}

		void CheckForExistingComponents() {
			var checks = new List<bool> {
				TryGetComponent(typeof(ProceduralMeshSolver),        out _),
				TryGetComponent(typeof(ProceduralTileSolver),        out _),
				TryGetComponent(typeof(ProceduralMapSolver),         out _),
				TryGetComponent(typeof(ProceduralPathfindingSolver), out _),
				TryGetComponent(typeof(ProceduralScaler),            out _),
				TryGetComponent(typeof(ProceduralGenerationEvents),  out _),
				TryGetComponent(typeof(ProceduralMapStateMachine),   out _),
				TryGetComponent(typeof(ProceduralUtility),           out _),
				TryGetComponent(typeof(ProceduralSceneBounds),       out _),
				TryGetComponent(typeof(ProceduralMapHandler),        out _),
				TryGetComponent(typeof(MeshFilter),                  out _),
				TryGetComponent(typeof(MeshRenderer),                out _)
			};

			foreach (var b in checks) {
				if (b)
					continue;

				_shouldShowRemoveButton = false;
				return;
			}

			_shouldShowRemoveButton = true;
		}

		void OnValidate() {
			CheckForExistingComponents();
		}
	}
}