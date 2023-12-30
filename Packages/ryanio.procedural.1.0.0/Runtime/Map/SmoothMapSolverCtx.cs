// ProceduralGeneration

namespace ProceduralGeneration {
	internal readonly struct SmoothMapSolverCtx {
		internal Dimensions Dimensions          { get; }
		// internal int        UpperNeighborLimit  { get; }
		// internal int        LowerNeighborLimit  { get; }
		internal int        SmoothingIterations { get; }

		internal SmoothMapSolverCtx(
			Dimensions dimensions,
			// int upperNeighborLimit,
			// int lowerNeighborLimit,
			int smoothingIterations) {
			Dimensions          = dimensions;
			// UpperNeighborLimit  = upperNeighborLimit;
			// LowerNeighborLimit  = lowerNeighborLimit;
			SmoothingIterations = smoothingIterations;
		}
	}
}