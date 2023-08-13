// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal partial class GenerationActions {
		public GameObject        GetColliderGameObject() => ColliderGameObject;
		public TileMapDictionary GetTilemapDictionary()  => TileMapDictionary;
		public Grid              GetGrid()               => Grid;
		public GameObject        GetOwner()              => _owner.Go;
		public TileHashset       GetTileHashset()        => _tileHashset;
	}
}