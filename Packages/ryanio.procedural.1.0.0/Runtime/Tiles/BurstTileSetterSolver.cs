// ProceduralGeneration

using System;
using Unity.Jobs;

namespace ProceduralGeneration {
	internal class BurstTileSetterSolver : TileTypeSolver {
		internal override void Set(ref int[,] map) {
			var handle = new JobHandle();
			var job    = new SetTilesJob(ref map);

			try {
				job.Schedule(1, 2, handle);

				while (true) {
					if (handle.IsCompleted)
						break;
				}

				handle.Complete();
			}
			catch (Exception e) {
				job.Dispose();
				throw;
			}
		}

		public static bool IsBoundary(int mapWidth, int mapHeight, int x, int y) =>
			x <= 0             ||
			y <= 0             ||
			x == mapWidth  - 1 ||
			y == mapHeight - 1;
	}
}