using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration {
	public abstract class MeshTriangulationSolver {
		public          List<List<int>>                 Outlines   { get; protected set; }
		public          Mesh                            SolvedMesh { get; protected set; }
		public abstract Tuple<List<int>, List<Vector3>> Triangulate(int[,] mapBorder);
	}
}