using Source.Events;

namespace Engine.Procedural.Runtime {
	internal class InternalEventProxy : IEngineEvent {
		public void AddListener<TAdd>(IEngineEventListener<TAdd> listener) where TAdd : struct {
			EngineEvent.AddListener(listener);
		}

		public void RemoveListener<TRemove>(IEngineEventListener<TRemove> listener) where TRemove : struct {
			EngineEvent.RemoveListener(listener);
		}

		public void TriggerEvent<TEvent>(TEvent newEvent) where TEvent : struct {
			EngineEvent.TriggerEvent(newEvent);
		}
	}
}