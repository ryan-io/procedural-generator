// Engine.Procedural

namespace Engine.Procedural {
	public readonly struct EnsureMapFitsOnStack {
		public void Ensure(ProceduralConfig config) {
			var borderSize = config.BorderSize;

			if (borderSize <= 1) {
				return;
			}

			var height = config.Columns;
			var width  = config.Rows;

			if (height + borderSize + 1 > Constants.MAP_DIMENSION_LIMIT)
				config.Columns = height - borderSize + 1;
			
			if (height + borderSize + 1 > Constants.MAP_DIMENSION_LIMIT)
				config.Rows  = width  - borderSize + 1;
		}
	}
}