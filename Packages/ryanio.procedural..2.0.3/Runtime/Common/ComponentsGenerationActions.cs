// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal partial class Actions {
		public GameObject        GetColliderGameObject() => ColliderGameObject;
		public TileMapDictionary GetTilemapDictionary()  => TileMapDictionary;
		public TileDictionary    GetTileDictionary()     => ProceduralConfig.TileDictionary;
		public Grid              GetGrid()               => Grid;
		public GameObject        GetOwner()              => _owner.Go;
		public TileHashset       GetTileHashset()        => _tileHashset;
	}
}