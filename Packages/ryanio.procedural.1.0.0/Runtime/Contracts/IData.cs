// ProceduralGeneration

using System.Collections.Generic;

namespace ProceduralGeneration {
	internal interface IData {
		IReadOnlyList<Room> GetRooms();
		SeedInfo            GetSeed();
		MeshData            GetMeshData();
		Coordinates         GetCoordinates();
		void                SetCoords(Coordinates coordinates);
		void                SetMeshData(MeshData meshData);
	}
}