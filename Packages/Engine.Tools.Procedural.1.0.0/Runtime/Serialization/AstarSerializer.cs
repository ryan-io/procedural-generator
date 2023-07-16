using BCL;
using Engine.Tools.Serializer;
using Pathfinding.Serialization;

namespace Engine.Procedural {
	public class AstarSerializer {
		StopWatchWrapper StopWatch { get; }

		/// <summary>
		///     Please ensure that your active graph data has been scanned.
		/// </summary>
		public void SerializeCurrentAstarGraph(SerializerSetup setup, string name) {
			if (string.IsNullOrWhiteSpace(name)) name = "DefaultAstar";

			var settings = new SerializeSettings {
				nodes          = true,
				editorSettings = true
			};

			var bytes            = AstarPath.active.data.SerializeGraphs(settings);
			var serializationJob = new Serializer.TxtJob(setup, name, bytes, setup.FileFormat);
			Serializer.SaveBytesData(serializationJob, true);
		}

		public void DeserializeAstarGraph(Job job) {
			var output =
				job.DataPath                +
				Constants.SAVE_ASTAR_PREFIX +
				job.NameOfMap               +
				Constants.UNDERSCORE        +
				job.Seed                    +
				Constants.UID               +
				job.Iteration               +
				Constants.TXT_FILE_TYPE;

			var hasData = Serializer.TryLoadBytesData(output, out var data);

			if (hasData)
				AstarPath.active.data.DeserializeGraphs(data);
			else {
				GenLogging.LogWithTimeStamp(
					LogLevel.Warning,
					StopWatch.TimeElapsed,
					Message.CANNOT_GET_SERIALIZED_ASTAR_DATA + output,
					Constants.DESERIALIZE_ASTAR_CTX
				);
			}
		}

		public readonly struct Job {
			public int    Iteration { get; }
			public string DataPath  { get; }
			public string Seed      { get; }
			public string NameOfMap { get; }

			public Job(string dataPath, string seed, int iteration, string nameOfMap) {
				DataPath  = dataPath;
				Seed      = seed;
				Iteration = iteration;
				NameOfMap = nameOfMap;
			}
		}

		public AstarSerializer(StopWatchWrapper stopWatch) {
			StopWatch = stopWatch;
		}
	}
}