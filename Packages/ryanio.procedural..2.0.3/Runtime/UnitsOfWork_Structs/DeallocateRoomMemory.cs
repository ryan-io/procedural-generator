// Engine.Procedural

using JetBrains.Annotations;

namespace Engine.Procedural.Runtime {
	public readonly struct DeallocateRoomMemory {
		public void Deallocate([CanBeNull] RegionRemovalSolver solver) {
			solver?.ResetRooms();
		}	
	}
}