using System;
using UnityEngine.Events;

namespace Engine.Procedural {
	[Serializable]
	public class OnDataGenerated : UnityEvent<MapGenerationData> {
	}
}