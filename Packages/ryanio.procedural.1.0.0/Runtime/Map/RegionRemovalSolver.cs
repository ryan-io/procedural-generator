using System.Collections.Generic;
using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal abstract class RegionSolver {
		internal abstract List<Room> Remove(Span2D<int> primarySpan);
	}
}