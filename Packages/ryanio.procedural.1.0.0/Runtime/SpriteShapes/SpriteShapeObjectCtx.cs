// ProceduralGeneration

using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	internal class SpriteShapeObjectCtx {
		internal GameObject            Owner      { get; }
		internal SpriteShapeController Controller { get; }

		public SpriteShapeObjectCtx(GameObject owner, SpriteShapeController controller) {
			Owner      = owner;
			Controller = controller;
		}
	}
}