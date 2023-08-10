// Engine.Procedural

using JetBrains.Annotations;

namespace ProceduralGeneration {
	public readonly struct DeallocateRoomMemory {
		public void Deallocate([CanBeNull] RegionRemovalSolver solver) {
			solver?.ResetRooms();
		}	
	}
}