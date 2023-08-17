using UnityEngine;

namespace ProceduralGeneration {
	public static class Constants {
		public static class Layer {
			public const string OBSTACLES = "Obstacles";
			public const string BOUNDARY  = "Boundary";
		}

		public static class SortingLayer {
			public const string GROUND           = "Ground";
			public const string BOUNDARY         = "Boundary";
			public const string OBSTACLES        = "Obstacles";
			public const string BACKGROUND       = "Background";
			public const string FOREGROUND_ONE   = "Foreground - 1";
			public const string FOREGROUND_TWO   = "Foreground - 2";
			public const string FOREGROUND_THREE = "Foreground - 3";
			public const string CHARACTERS       = "Characters";
			public const string Y_SORTING        = "Y-Sorting";
		}

		public const string JSON_FILE_TYPE = ".json";

		public const string ITERATION_LABEL = "Iteration";

		public const string SPRITE_SHAPE_SAVE_PREFIX = "SpriteShape_";

		public const string COLLIDER_COORDS_SAVE_PREFIX = "ColliderCoords_";

		public const string SPRITE_BOUNDARY_KEY = "boundary-shape key ";

		public const string SPRITE_BOUNDARY_CTX = "SpriteBoundary";

		public const string PROCEDURAL_MESH_NAME = "procedural-mesh";

		public const string ASSETS_FOLDER = "Assets/";

		public const string PREFAB_FILE_TYPE = ".prefab";

		public const string SAVE_COLLIDERS_PREFIX = "Colliders_";

		public const string BACKSLASH = "/";

		public const string SERIALIZED_DATA_FOLDER = "SerializedData/";

		public const string SEED_TRACKER_FILE_NAME = "seedTracker";

		public const string ASTAR_SERIALIZE_PREFIX = "AstarGraph_";

		public const string SAVE_MAP_PREFIX = "Map_";

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

		public static readonly Color[] Color = {
			UnityEngine.Color.red, UnityEngine.Color.green, UnityEngine.Color.cyan, UnityEngine.Color.yellow,
			UnityEngine.Color.magenta, UnityEngine.Color.blue, UnityEngine.Color.white
		};

		public static Color[] GetColorArray() {
			var output = new[] {
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