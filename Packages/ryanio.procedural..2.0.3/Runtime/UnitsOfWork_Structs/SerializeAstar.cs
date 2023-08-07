// ProceduralGeneration

using BCL.Serialization;
using Pathfinding.Serialization;
using UnityBCL.Serialization;

namespace Engine.Procedural.Runtime {
	public readonly struct SerializeAstar {
		public void Serialize(SerializeSettings settings, string name, string path) {
			// var location = UnitySaveLocation.GetDefault;
			// var path     = location.GetFilePath(name, Constants.JSON_FILE_TYPE, Constants.ASTAR_FILE_PREFIX);

			var serializer       = new Serializer();
			var bytes            = AstarPath.active.data.SerializeGraphs(settings);
			var serializationJob = new SerializeJob.Json(name, path);
			serializer.SerializeAndSaveJson(bytes, serializationJob);
		}
	}
}