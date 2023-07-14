using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public class ProceduralMeshSolverMonobehaviorModel {
#region Serialization

		[SerializeField] [Title("Mesh Serialization")]
		MeshSerialization _meshSerialization;

#endregion

#region Serialized Required Monobehaviors	---------------------------------------->

		[field: SerializeField]
		[field: Title("Required Monobehaviors")]
		[field: Required]
		public ProceduralMapSolver ProceduralMapSolver { get; set; }

		[field: SerializeField]
		[field: Required]
		public MeshFilter MeshFilter { get; set; }

#endregion

#region Procedural Configurations			---------------------------------------->

		[field: SerializeField]
		[field: Title("Mesh Solver Configuration")]
		[field: Required]
		public ProceduralMeshSolverConfiguration ProceduralMeshSolverConfiguration { get; private set; }

		[Button(ButtonSizes.Medium)]
		[GUIColor(255 / 255f, 41 / 255f, 84 / 255f)]
		[ShowIf("@ProceduralMeshSolverConfiguration != null")]
		void SetIniToNull() => ProceduralMeshSolverConfiguration = null;

#endregion
	}
}