using System;
using UnityBCL;
using UnityEngine.Events;

namespace Engine.Procedural.Runtime {
	[Serializable]
	public class EventDictionary : SerializedDictionary<ProcessStep, UnityEvent> {
		
	}
}