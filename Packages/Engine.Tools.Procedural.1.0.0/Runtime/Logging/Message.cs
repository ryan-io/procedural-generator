namespace Engine.Procedural.Runtime {
	internal static class Message {
		internal const string UPDATED_SEED_TRACKER = "Updated the procedural seed tracker.";

		internal const string CTX_ERROR = "Error";

		internal const string STACK_OVERFLOW_ERROR =
			"StackOverflow exception caught. BorderMap stackalloc is most likely the culprit.";

		internal const string PERCENTAGE_WALLS = "Percentage of tiles that should be 'walls'";

		internal const string HEAP_ALLOCATION = "Allocated heap memory: ";

		internal const string RESET_GENERATION = "Reset All Procedural Generation?";

		internal const string CANNOT_CAST_GRAPH_ERROR = "Could not successfully cast to NavGraph.";

		internal const string CANNOT_GET_SERIALIZED_ASTAR_DATA = "Could not find serialized astar data with the output ";

		internal const string MAP_WILL_BE_RESIZED = "Max size will be modified if border-size > 1";

		internal const string NO_NAME_FOUND = "Please verify you are serializing the correct map. Could not find name: ";
	}
}