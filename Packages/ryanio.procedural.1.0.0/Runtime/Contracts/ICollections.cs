// ProceduralGeneration

using System.Collections.Generic;

namespace ProceduralGeneration {
	internal interface ICollections {
		TileMapDictionary GetTilemapDictionary();
		TileDictionary    GetTileDictionary();
		TileHashset       GetTileHashset();
		EventDictionary   GetSerializedEvents();

		void SetTileMapDictionary(TileMapDictionary tileMapDictionary);
		void SetRooms(IReadOnlyList<Room> rooms);
	}
}