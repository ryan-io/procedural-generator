using System;
using UnityBCL;
using UnityEngine.Events;

namespace Engine.Procedural {
	[Serializable]
	public class EventDictionary : SerializedDictionary<ProcessStep, UnityEvent> {
		
	}
}