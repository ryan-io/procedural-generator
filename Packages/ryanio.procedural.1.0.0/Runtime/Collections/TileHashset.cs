using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralGeneration {
	public class TileHashset : HashSet<TileRecord> {
		public TileRecord this[Vector2Int coordinate]
			=> this.FirstOrDefault(t => t.Coordinate == coordinate);
		
		public TileRecord this[Vector2 coordinate] {
			get {
				var castedCoord = new Vector2Int((int)coordinate.x, (int)coordinate.y);
				return this.FirstOrDefault(t => t.Coordinate == castedCoord);
			}
		}

		public Vector3 this[int x, int y] {
			get {
				var record = this[new Vector2Int(x, y)];
				if (record == null)
					return Vector3.zero;
				
				return new Vector3(record.Coordinate.x, record.Coordinate.y, 0);
			}
		}
		
		public Vector3 this[float x, float y] {
			get {
				var record = this[new Vector2Int((int)x, (int)y)];
				if (record == null)
					return Vector3.zero;
				
				return new Vector3(record.Coordinate.x, record.Coordinate.y, 0);
			}
		}
		
		
	}
}