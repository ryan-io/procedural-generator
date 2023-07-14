using UnityEngine;

namespace Engine.Procedural {
	public record TileRecord(Vector2Int Coordinate, TileMask Bit, bool IsMapBoundary, bool IsLocalBoundary);
}