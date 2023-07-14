using System;
using UnityBCL;
using UnityEngine.Tilemaps;

namespace Engine.Procedural {
	[Serializable]
	public class TileDictionary : SerializedDictionary<string, TileBase> {
		public static TileDictionary GetDefault() {
			var dict = new TileDictionary() {
				{ TileId.GROUND, default },
				{ TileId.BOUNDARY, default },
				{ TileId.NORTH_EAST_ANGLE, default },
				{ TileId.NORTH_WEST_ANGLE, default },
				{ TileId.SOUTH_EAST_ANGLE, default },
				{ TileId.SOUTH_WEST_ANGLE, default },
				{ TileId.NORTH_POCKET, default },
				{ TileId.SOUTH_POCKET, default },
				{ TileId.EAST_POCKET, default },
				{ TileId.WEST_POCKET, default },
				{ TileId.WEST_POCKET, default },
				{ TileId.SOUTH_OUTLINE, default },
			};

			return dict;
		}
	}
}