using System.Threading;
using Cysharp.Threading.Tasks;

namespace Engine.Procedural {
	/// <summary>
	///     Used to manually invoke procedural generation in the editor. All of these method should be implemented.
	/// </summary>
	public interface ICreation {
		UniTask Init(CancellationToken token);
		UniTask Enable(CancellationToken token);
		UniTask Begin(CancellationToken token);
		UniTask End(CancellationToken token);
		UniTask Dispose(CancellationToken token);
	}
}