// ProceduralGeneration

using JetBrains.Annotations;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	///  Can also add an initial material to this ctx if you want to use it for rendering.
	/// </summary>
	internal readonly struct MeshSolverCtx {
		internal             GameObject Owner            { get; }
		[CanBeNull] internal Material   MeshMaterial     { get; }
		internal             string     SerializableName { get; }

		public MeshSolverCtx(GameObject owner, Material meshMaterial, string serializableName) {
			Owner            = owner;
			MeshMaterial     = meshMaterial;
			SerializableName = serializableName;
		}
	}
}