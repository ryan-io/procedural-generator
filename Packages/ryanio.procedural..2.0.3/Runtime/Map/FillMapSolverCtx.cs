// ProceduralGeneration

namespace ProceduralGeneration {
	internal readonly struct FillMapSolverCtx {
		internal int SeedHash           { get; }
		internal int WallFillPercentage { get; }

		internal FillMapSolverCtx(int wallFillPercentage, int seedHash) {
			WallFillPercentage = wallFillPercentage;
			SeedHash           = seedHash;
		}
	}
}