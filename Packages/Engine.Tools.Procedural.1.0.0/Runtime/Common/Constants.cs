﻿using UnityEngine;

namespace Engine.Procedural.Runtime {
	public static class Constants {
		public static class Layers {
			public const string OBSTACLES = "Obstacles";
		}

		public const string SPRITE_SHAPE_SAVE_PREFIX = "SpriteShape_";
		
		public const string SPRITE_BOUNDARY_KEY = "boundary-shape key ";

		public const string SPRITE_BOUNDARY_CTX = "SpriteBoundary";
		
		public const string PROCEDURAL_MESH_NAME = "procedural-mesh";

		public const string ASSETS_FOLDER = "Assets/";
		
		public const string PREFAB_FILE_TYPE = ".prefab";

		public const string SAVE_COLLIDERS_PREFIX = "Colliders_";
		
		public const string BACKSLASH = "/";

		public const string SERIALIZED_DATA_FOLDER = "SerializedData/";

		public const string SEED_TRACKER_FILE_NAME = "seedTracker";

		public const string SAVE_ASTAR_PREFIX = "AstarGraph_";

		public const string SAVE_SEED_PREFIX = "MapSeed_";
		
		public const string SAVE_MESH_PREFIX = "Mesh_";

		public const string UID = "_uid-";

		public const string TILE_LABEL = "Tile";

		public const string PATHFINDING_MESH_LABEL = "Pathfinding Mesh";

		public const string ASTAR_GRAPH_NAME = "ProceduralGridGraph";

		public const string UNDERSCORE = "_";

		public const string SPACE = " ";

		public const string PATHFINDING_TAG = "Pathfinding";

		public const string DESERIALIZE_ASTAR_CTX = "DeserializeAstarGraph_";

		public const string SERIALIZE_SEED_CTX = "SerializedSeedTracker_";

		public const string TXT_FILE_TYPE = ".txt";

		public const int CELL_SIZE = 1;

		public const int MAP_DIMENSION_LIMIT = 320;

		public const float FLOATING_POINT_ERROR = 0.0005f;
		
		public static readonly Color[] Color =
			{ UnityEngine.Color.red, UnityEngine.Color.green, UnityEngine.Color.cyan, UnityEngine.Color.yellow, UnityEngine.Color.magenta, UnityEngine.Color.blue, UnityEngine.Color.white };

		public static Color[] GetColorArray() {
			var output = new [] {
				UnityEngine.Color.red, 
				UnityEngine.Color.green, 
				UnityEngine.Color.cyan, 
				UnityEngine.Color.yellow, 
				UnityEngine.Color.magenta, 
				UnityEngine.Color.blue, 
				UnityEngine.Color.white
			};

			return output;
		}
	}
}