

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	public class RunTimeLocalAABBUpdate : MonoBehaviour {
		[Button]
		void UpdateAABB() {
				var controller = gameObject.GetComponent<SpriteShapeController>();
				var rend       = gameObject.GetComponent<SpriteShapeRenderer>();

				Debug.Log("About to update local AABB!");
				UpdateLocalAABB.Update(rend, controller, transform);
		}
	}
}