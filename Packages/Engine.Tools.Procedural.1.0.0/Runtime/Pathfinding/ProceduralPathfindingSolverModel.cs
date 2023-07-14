using BCL;
using UnityEngine;

namespace Engine.Procedural {
	public class ProceduralPathfindingSolverModel {
		public ProceduralPathfindingSolverModel(string startId, string completeId)
			=> Observables = new ObservablesCollection(true) {
				startId, completeId
			};

		public ObservablesCollection Observables               { get; }
		public Mesh                  PathfindingMesh           { get; set; }
		public GameObject            PathfindingMeshGameObject { get; set; }
	}
}