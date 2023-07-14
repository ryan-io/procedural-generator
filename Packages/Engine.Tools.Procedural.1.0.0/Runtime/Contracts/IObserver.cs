// Engine.Procedural

using BCL;

namespace Engine.Procedural {
	public interface IObserver {
		ObservableCollection<string> Observables { get; }
	}
}