// ProceduralGeneration

using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	internal interface IComponents {
		GameObject                         GetOwner();
		GameObject                         GetColliderGameObject();
		IReadOnlyList<GraphColliderCutter> GetGraphColliderCutters();
		Grid                               GetGrid();

		void SetColliderGameObject(GameObject o);
		void SetGrid(Grid grid);
	}
}