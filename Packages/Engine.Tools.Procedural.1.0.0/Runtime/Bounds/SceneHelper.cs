namespace Engine.Procedural {
	public static class SceneHelper {
		public static int[] GetTotalMapDimensions(MapDimensionsModel dim) {
			var xDimension = dim.CellSize * dim.MapWidth;
			var yDimension = dim.CellSize * dim.MapHeight;
			return new[] { xDimension, yDimension };
		}
	}
}