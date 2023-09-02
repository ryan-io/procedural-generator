

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	public class RunTimeLocalAABBUpdate : MonoBehaviour {
		[Button]
		void UpdateAABB() {
				var                                 controller = gameObject.GetComponent<SpriteShapeController>();
				var                                 rend       = gameObject.GetComponent<SpriteShapeRenderer>();
				var                                 job        = controller.BakeMesh();
				UnityEngine.Rendering.CommandBuffer rc         = new UnityEngine.Rendering.CommandBuffer();

				var rt = RenderTexture.GetTemporary(256, 256, 0, RenderTextureFormat.ARGB32);

				Graphics.SetRenderTarget(rt);

				rc.DrawRenderer(rend, rend.sharedMaterial);
if (!rend.isVisible)
	Debug.LogWarning("Renderer is not visible! " + rend.name);
				Graphics.ExecuteCommandBuffer(rc);
				job.Complete();
				Debug.Log("About to update local AABB!");
				UpdateLocalAABB.Update(rend, controller, transform);
		}
	}
}