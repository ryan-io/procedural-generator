// Engine.Procedural

namespace Engine.Procedural {
	public readonly struct EnsureMapFitsOnStack {
		public void Ensure(ProceduralConfig config) {
			var borderSize = config.BorderSize;

			if (borderSize <= 1) {
				return;
			}

			var height = config.Height;
			var width  = config.Width;

			if (height + borderSize + 1 > Constants.MAP_DIMENSION_LIMIT)
				config.Height = height - borderSize + 1;
			
			if (height + borderSize + 1 > Constants.MAP_DIMENSION_LIMIT)
				config.Width  = width  - borderSize + 1;
		}
	}
}