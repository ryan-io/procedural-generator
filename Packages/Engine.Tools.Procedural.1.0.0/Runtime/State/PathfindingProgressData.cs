using Unity.Mathematics;
using UnityEngine.Tilemaps;

namespace Engine.Procedural {
	public readonly struct PathfindingProgressData {
		public PathfindingProgressData(int2 mapDimensions, float cellSize, Tilemap boundaryTilemap,
			Tilemap groundTilemap, TileHashset tileHashset, ProceduralGenerator proceduralGenerator, string seed,
			int iteration, string nameOfMap) {
			MapDimensions        = mapDimensions;
			CellSize             = cellSize;
			BoundaryTilemap      = boundaryTilemap;
			GroundTilemap        = groundTilemap;
			TileHashset          = tileHashset;
			ProceduralGenerator = proceduralGenerator;
			Seed                 = seed;
			Iteration            = iteration;
			NameOfMap            = nameOfMap;
		}

		public string               NameOfMap            { get; }
		public TileHashset          TileHashset          { get; }
		public Tilemap              BoundaryTilemap      { get; }
		public Tilemap              GroundTilemap        { get; }
		public int2                 MapDimensions        { get; }
		public float                CellSize             { get; }
		public ProceduralGenerator ProceduralGenerator { get; }
		public string               Seed                 { get; }
		public int                  Iteration            { get; }
	}
}