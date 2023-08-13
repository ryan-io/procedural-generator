using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using Sirenix.Utilities;

namespace ProceduralGeneration {
	internal abstract class RegionRemovalSolver {
		internal abstract List<Room> Rooms { get; set; }
		internal abstract void       Remove(Span2D<int> primarySpan);

		internal void ResetRooms() {
			if (Rooms.IsNullOrEmpty())
				return;

			Rooms.Clear();
		}
	}
}