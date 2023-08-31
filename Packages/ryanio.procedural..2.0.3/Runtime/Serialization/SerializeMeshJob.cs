// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct SerializeMeshJob {
		internal string                    Name        { get; }
		internal (string raw, string full) Directories { get; }
		internal Mesh                      Mesh        { get; }

		public SerializeMeshJob(string name, (string raw, string full) directories, Mesh mesh) {
			Name        = name;
			Directories = directories;
			Mesh   = mesh;
		}
	}
}