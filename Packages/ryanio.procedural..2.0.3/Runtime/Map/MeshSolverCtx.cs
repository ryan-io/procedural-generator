// ProceduralGeneration

using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///  Can also add an initial material to this ctx if you want to use it for rendering.
	/// </summary>
	internal readonly struct MeshSolverCtx {
		internal GameObject Owner            { get; }
		internal string     SerializableName { get; }

		public MeshSolverCtx(GameObject owner, string serializableName) {
			Owner = owner;
			SerializableName = serializableName;
		}
			
	}
}