using UnityBCL;
using UnityEngine;

namespace Engine.Procedural {
	public static class ProcGizmos {
		public static void DrawCircle(Vector3 position, float radius) {
			DebugExt.DrawCircle(position, Vector3.forward, Color.magenta, radius);
		}
	}
}