// Engine.Procedural

using JetBrains.Annotations;

namespace Engine.Procedural {
	public readonly struct DeallocateRoomMemory {
		public void Deallocate([CanBeNull] RegionRemovalSolver solver) {
			solver?.ResetRooms();
		}	
	}
}