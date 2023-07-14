// Engine.Procedural

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Engine.Procedural {
	[Serializable]
	public class ProceduralMapStateMachineMonobehaviorModel {
		[field: SerializeField]
		[field: Title("Required Monobehaviors")]
		[field: Required]
		public ProceduralGenerator ProceduralGenerator { get; set; }
	}
}