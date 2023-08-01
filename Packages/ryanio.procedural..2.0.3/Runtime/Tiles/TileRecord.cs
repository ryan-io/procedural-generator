﻿using UnityEngine;

namespace Engine.Procedural.Runtime {
	public record TileRecord(Vector2Int Coordinate, TileMask Bit, bool IsMapBoundary, bool IsLocalBoundary) {
		public Vector2Int Coordinate      { get; } = Coordinate;
		public TileMask   Bit             { get; } = Bit;
		public bool       IsMapBoundary   { get; } = IsMapBoundary;
		public bool       IsLocalBoundary { get; } = IsLocalBoundary;
	}
}