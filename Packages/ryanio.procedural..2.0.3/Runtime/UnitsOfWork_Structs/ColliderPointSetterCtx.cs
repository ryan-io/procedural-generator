// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct ColliderPointSetterCtx {
		internal GameObject  ColliderGo { get; }
		internal TileHashset Hashset    { get; }
		internal Dimensions  Dimensions { get; }
		internal int         BorderSize { get; }
		internal float       Radius     { get; }

		public ColliderPointSetterCtx(
			GameObject colliderGo, 
			TileHashset hashset, 
			Dimensions dimensions, 
			int borderSize, float radius) {
			ColliderGo = colliderGo;
			Hashset    = hashset;
			Dimensions = dimensions;
			BorderSize = borderSize;
			Radius     = radius;
		}
	}
}