using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using Sirenix.Utilities;

namespace ProceduralGeneration {
	public abstract class RegionRemovalSolver {
		public abstract List<Room> Rooms { get; protected set; }
		public abstract void       Remove(Span2D<int> primarySpan);

		public void ResetRooms() {
			if (Rooms.IsNullOrEmpty())
				return;
			
			Rooms.Clear();
		}
	}
}