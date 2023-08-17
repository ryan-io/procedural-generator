// ProceduralGeneration

using BCL;
using UnityEngine;

namespace ProceduralGeneration {
	internal partial class Actions : IActions {
		public ProceduralConfig  ProceduralConfig  { private get; set; }
		public SpriteShapeConfig SpriteShapeConfig { private get; set; }

		public float GetTimeElapsed() => _stopWatch.TimeElapsed;

		public void SetColliderGameObject(GameObject o)                       => ColliderGameObject = o;
		public void SetTileMapDictionary(TileMapDictionary tileMapDictionary) => TileMapDictionary = tileMapDictionary;
		public void SetGrid(Grid grid)                                        => Grid = grid;
		public void SetMeshData(MeshData meshData)                            => MeshData = meshData;

		GameObject        ColliderGameObject { get; set; }
		TileMapDictionary TileMapDictionary  { get; set; }
		Grid              Grid               { get; set; }
		MeshData          MeshData           { get; set; }

		/// <summary>
		///   Constructor, does not initialize ProceduralConfig or SpriteShapeConfig.
		/// </summary>
		internal Actions(IOwner owner) {
			_owner       = owner;
			_tileHashset = new TileHashset();
			_stopWatch   = new StopWatchWrapper(true);
			_logger      = new GenLogging(_stopWatch);
		}

		readonly IOwner             _owner;
		readonly IProceduralLogging _logger;
		readonly TileHashset        _tileHashset;
		readonly StopWatchWrapper   _stopWatch;
	}
}