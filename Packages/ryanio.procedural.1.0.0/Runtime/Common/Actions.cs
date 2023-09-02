// ProceduralGeneration

using System.Collections.Generic;
using BCL;
using UnityEngine;

namespace ProceduralGeneration {
	internal partial class Actions : IActions {
		public ProceduralConfig  ProceduralConfig  { private get; set; }
		public SpriteShapeConfig SpriteShapeConfig { private get; set; }

		public SpriteShapeConfig                  GetSpriteShapeConfig()    => SpriteShapeConfig;
		public IReadOnlyList<GraphColliderCutter> GetGraphColliderCutters() => ProceduralConfig.ColliderCutters;

		public float GetTimeElapsed() => _stopWatch.TimeElapsed;

		public bool GetShouldSerializePathfinding() => ProceduralConfig.ShouldSerializePathfinding;

		public bool GetShouldSerializeMapPrefab() => ProceduralConfig.ShouldSerializeMapPrefab;

		public bool GetShouldSerializeSpriteShape() => ProceduralConfig.ShouldSerializeSpriteShape;

		public bool GetShouldSerializeColliderCoords() => ProceduralConfig.ShouldSerializeColliderCoords;
		public bool GetShouldSerializeMesh()           => ProceduralConfig.ShouldSerializeMesh;

		public void SetColliderGameObject(GameObject o) => ColliderGameObject = o;
		public void SetTileMapDictionary(TileMapDictionary tileMapDictionary) => TileMapDictionary = tileMapDictionary;
		public void SetGrid(Grid grid) => Grid = grid;
		public void StopTimer() => _stopWatch.Stop();
		public string GetMapName() => ProceduralConfig.Name;
		public bool GetShouldRenderTiles() => ProceduralConfig.ShouldRenderTiles;
		public   Material           GetMeshMaterial() => ProceduralConfig.MeshMaterial;

		public bool GetShouldDeserializePathfinding() => ProceduralConfig.ShouldSerializePathfinding;

		public bool GetShouldDeserializeMapPrefab() => ProceduralConfig.ShouldDeserializeMapPrefab;

		public bool GetShouldDeserializeSpriteShape() => ProceduralConfig.ShouldDeserializeSpriteShape;

		public bool GetShouldDeserializeColliderCoords() => ProceduralConfig.ShouldDeserializeColliderCoords;
		public bool GetShouldDeserializeMesh()           => ProceduralConfig.ShouldDeserializeMesh;

		public EventDictionary     GetSerializedEvents()               => ProceduralConfig.SerializedEvents;
		public IReadOnlyList<Room> GetRooms()                          => Rooms;
		public void                SetRooms(IReadOnlyList<Room> rooms) => Rooms = rooms;

		public void SetMeshData(MeshData meshData) => MeshData = meshData;

		GameObject        ColliderGameObject { get; set; }
		TileMapDictionary TileMapDictionary  { get; set; }
		Grid              Grid               { get; set; }
		MeshData          MeshData           { get; set; }
		IReadOnlyList<Room>        Rooms              { get; set; }

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