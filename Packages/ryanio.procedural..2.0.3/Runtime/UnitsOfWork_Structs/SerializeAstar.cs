// ProceduralGeneration

using BCL.Serialization;
using Pathfinding.Serialization;

namespace ProceduralGeneration {
	public readonly struct SerializeAstar {
		public void Serialize(SerializeSettings settings, string name, string path) {
			name = Constants.ASTAR_SERIALIZE_PREFIX + name;
			
			var serializer       = new Serializer();
			var bytes            = AstarPath.active.data.SerializeGraphs(settings);
			var serializationJob = new SerializeJob.Json(name, path);
			serializer.SerializeAndSaveJson(bytes, serializationJob);
		}
	}
}