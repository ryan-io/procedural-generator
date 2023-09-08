// ProceduralGeneration

using CommunityToolkit.HighPerformance;
using Sirenix.OdinInspector;
using Unity.Jobs;
using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration.Gizmos {
	public class ProceduralDebuggingTools : MonoBehaviour {
		[Button]
		void ZoomSceneCamera(float size) {
			Temporary.ZoomSceneCamera(size);
		}

		[Button]
		void ValidateRendererIsVisible() {
			if (!Generator)
				return;

			var renderers = Generator.GetComponentsInChildren<SpriteShapeRenderer>();

			if (renderers.IsEmptyOrNull())
				return;

			var logger = new UnityLogging();

			foreach (var rend in renderers) {
				if (!rend)
					continue;

				logger.Log(rend.name + "visibility is " + rend.isVisible);
			}
		}

		[field: SerializeField, Required] public ProceduralGenerator Generator { get; private set; }
	}
}