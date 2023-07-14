using System.Threading;
using Cysharp.Threading.Tasks;

namespace Engine.Procedural {
	public interface IProgress {
		UniTask Progress_PopulatingMap(CancellationToken token);
		UniTask Progress_SmoothingMap(CancellationToken token);
		UniTask Progress_CreatingBoundary(CancellationToken token);
		UniTask Progress_RemovingRegions(CancellationToken token);
		UniTask Progress_CompilingData(CancellationToken token);
		UniTask Progress_PreparingAndSettingTiles(CancellationToken token);
		UniTask Progress_GeneratingMesh(CancellationToken token);
		UniTask Progress_CalculatingPathfinding(PathfindingProgressData progressData, CancellationToken token);
	}
}