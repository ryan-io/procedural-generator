namespace Engine.Procedural.Runtime {
	public static class Message {
		public const string UPDATED_SEED_TRACKER = "Updated the procedural seed tracker.";
		
		public const string CTX_ERROR            = "Error";
		
		public const string STACK_OVERFLOW_ERROR =
			"StackOverflow exception caught. BorderMap stackalloc is most likely the culprit.";
		
		public const string PERCENTAGE_WALLS = "Percentage of tiles that should be 'walls'";

		public const string HEAP_ALLOCATION = "Allocated heap memory: ";

		public const string RESET_GENERATION = "Reset All Procedural Generation?";

		public const string CANNOT_CAST_GRAPH_ERROR = "Could not successfully cast to NavGraph.";

		public const string CANNOT_GET_SERIALIZED_ASTAR_DATA = "Could not find serialized astar data with the output ";
		
		public const string MAP_WILL_BE_RESIZED =  "Max size will be modified if border-size > 1";
	}
}