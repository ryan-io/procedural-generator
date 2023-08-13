// ProceduralGeneration

using System;

namespace ProceduralGeneration {
	internal static class ProceduralService {
		internal static FillMapSolver GetFillMapSolver(Func<FillMapSolver> constructor) => constructor.Invoke();

		internal static SmoothMapSolver GetSmoothMapSolver(Func<SmoothMapSolver> constructor) => constructor.Invoke();

		internal static RegionRemovalSolver GetRegionRemovalSolver(Func<RegionRemovalSolver> constructor)
			=> constructor.Invoke();
	}
}