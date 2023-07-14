using BCL;
using Unity.Mathematics;

namespace Engine.Procedural {
	public interface ISceneBounds {
		IObservable<int4> OnBoundaryDetermined { get; }
	}
}