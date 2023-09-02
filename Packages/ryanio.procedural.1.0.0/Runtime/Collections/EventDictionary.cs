using System;
using UnityBCL;
using UnityEngine.Events;

namespace ProceduralGeneration {
	[Serializable]
	public class EventDictionary : SerializedDictionary<ProcessStep, UnityEvent> {
		
	}
}