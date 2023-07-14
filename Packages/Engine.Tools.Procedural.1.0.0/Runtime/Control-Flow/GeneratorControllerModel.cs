using System;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public struct GeneratorControllerModel {
		[field: SerializeField] public ProceduralMapSolver Generator { get; private set; }
	}
}