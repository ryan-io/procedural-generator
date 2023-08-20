namespace ProceduralGeneration {
	internal static class Message {
		internal const string UPDATED_SEED_TRACKER = "Updated the procedural seed tracker.";

		internal const string CTX_ERROR = "Error";

		internal const string STACK_OVERFLOW_ERROR =
			"StackOverflow exception caught. BorderMap stackalloc is most likely the culprit.";

		internal const string PERCENTAGE_WALLS = "Percentage of tiles that should be 'walls'";

		internal const string HEAP_ALLOCATION = "Allocated heap memory: ";

		internal const string RESET_GENERATION = "Reset All Procedural Generation?";

		internal const string CANNOT_CAST_GRAPH_ERROR = "Could not successfully cast to NavGraph.";

		internal const string CANNOT_GET_SERIALIZED_ASTAR_DATA =
			"Could not find serialized astar data with the output ";

		internal const string MAP_WILL_BE_RESIZED = "Max size will be modified if border-size > 1";

		internal const string NO_NAME_FOUND =
			"Please verify you are serializing the correct map. Could not find name: ";

		internal const string NO_DATA_SPRITE_BOUNDARY =
			"Characteristics are currently null or empty. Please generate a map and try again.";

		internal const string START_SHAPE_BOUNDARY_GENERATION = "Beginning procedural border procedure";

		internal const string ALREADY_RUNNING = "Generator is currently running.";

		internal const string NAME_NOT_SERIALIZED =
			" is not serialized. Run the generator to create a seed and serialize it.";

		internal const string NOT_SET_TO_RUN = "Generator is not configured to generate or deserialize. Exiting.";

		internal const string CLEAN_COMPLETE = "Clean complete.";

		internal const string GENERATION_COMPLETE = "Generation complete.";

		internal const string DESERIALIZATION_COMPLETE = "Deserialization complete.";

		internal const string GENERATION_ERROR = "Generation error: ";

		internal const string DESERIALIZATION_ERROR = "Deserialization error: ";

		internal const string COULD_NOT_FIND_DIR = "Could not find a directory to delete for ";

		internal const string TIME_TO_FILL_MAP = "Total time to fill map: ";
		
		internal const string STATE_TO_CLEAN = "State changed to cleaning.";
		
		internal const string STATE_TO_INIT = "State changed to initializing.";
		
		internal const string STATE_TO_RUN = "State changed to running.";
		
		internal const string STATE_TO_COMPLETE = "State changed to completing.";
		
		internal const string STATE_TO_DISPOSE = "State changed to disposing.";
		
		internal const string STATE_TO_ERROR = "State changed to error.";

		internal const string CANT_SERIALIZE_MAP_GO = "Could not save map prefab. There may be one already serialized.";

		internal const string SERIALIZE_MAP_AT = "Serialized map at: ";
	}
}