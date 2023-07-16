using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Engine.Procedural {
	public class TileHashset : HashSet<TileRecord> {
		public TileRecord this[Vector2Int coordinate]
			=> this.FirstOrDefault(t => t.Coordinate == coordinate);
	}
}