using Source.Events;

namespace InventoryEngine {
	internal class InternalEventProxy : IEngineEvent {
		public void AddListener<TAdd>(IEngineEventListener<TAdd> listener) where TAdd : struct {
			EngineEvent.AddListener(listener);
		}

		public void RemoveListener<TRemove>(IEngineEventListener<TRemove> listener) where TRemove : struct {
			EngineEvent.RemoveListener(listener);
		}

		public void TriggerEvent<TTrigger>(TTrigger newEvent) where TTrigger : struct {
			EngineEvent.TriggerEvent(newEvent);
		}
	}
}