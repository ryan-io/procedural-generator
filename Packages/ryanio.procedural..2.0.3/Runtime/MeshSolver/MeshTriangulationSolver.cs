using System;
using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using UnityEngine;

namespace ProceduralGeneration {
	internal abstract class MeshTriangulationSolver {
		internal          List<List<int>>                 Outlines   { get; set; }
		internal          Mesh                            SolvedMesh { get; set; }
		internal abstract Tuple<List<int>, List<Vector3>> Triangulate(Span2D<int> map);
	}
}