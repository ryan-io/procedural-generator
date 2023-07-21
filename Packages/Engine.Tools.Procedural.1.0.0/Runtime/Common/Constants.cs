
using UnityEngine;

namespace Engine.Procedural {
	public static class Constants {
		public static class Layers {
			public const string OBSTACLES = "Obstacles";
		}

		public const string BACKSLASH = "/";

		public const string SERIALIZED_DATA_FOLDER_NAME = "SerializedData/";

		public const string SEED_TRACKER_FILE_NAME = "seedTracker";

		public const string SAVE_ASTAR_PREFIX = "AstarGraph_";

		public const string SAVE_SEED_PREFIX = "MapSeed_";

		public const string UID = "_uid-";

		public const string TILE_LABEL = "Tile";

		public const string MESH_LABEL = "Procedural Mesh";

		public const string PATHFINDING_MESH_LABEL = "Pathfinding Mesh";

		public const string ASTAR_GRAPH_NAME = "ProceduralGridGraph";

		public const string EDGE_COLLIDER_GO_NAME = "Procedural Edge Colliders";

		public const string UNDERSCORE = "_";

		public const string SPACE = " ";

		public const string PATHFINDING_TAG = "Pathfinding";

		public const string DESERIALIZE_ASTAR_CTX = "DeserializeAstarGraph_";

		public const string SERIALIZE_SEED_CTX = "SerializedSeedTracker_";

		public const string TXT_FILE_TYPE = ".txt";

		public const int CELL_SIZE = 1;

		public const int MAP_DIMENSION_LIMIT = 320;
		
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