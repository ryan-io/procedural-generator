using Unity.Mathematics;
using UnityEngine.Tilemaps;

namespace Engine.Procedural {
	public readonly struct GridGraphModel {
		public Tilemap GroundTileMap { get; }
		public int2    MapDimensions { get; }
		public int     CellSize      { get; }
		public float   NodeSize      { get; }

		public GridGraphModel(Tilemap groundTileMap, int cellSize, int2 mapDimensions, float nodeSize) {
			GroundTileMap = groundTileMap;
			CellSize      = cellSize;
			MapDimensions = mapDimensions;
			NodeSize      = nodeSize;
		}
	}
}