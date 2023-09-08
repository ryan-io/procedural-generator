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
		void TestSmoothMapJob(int iterations = 1) {
			const int rows = 1000;
			const int cols = 1000;

			var span = new Span2D<int>(new int[rows, cols]);
			
			for (var i = 0; i < rows * cols; i++) {
				var row = i / cols;
				var col = i % cols;

				// logic   
				span[row, col] = 0;
			}

			
			var job    = new SmoothMapJob(span);
			//var handle = job.Schedule(iterations, 2);					// -> IJobParallelFor
			var handle = job.Schedule(iterations, new JobHandle());	// -> IJobFor

			handle.Complete();

			Debug.Log(job.MapProcessed.Length);
			Debug.Log(job.Map.Length);
			Debug.Log("Total iterations: " + job.MapProcessedInt[0]);

			job.Dispose();
		}

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