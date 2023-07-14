// Engine.Procedural

using Engine.Procedural;
using Pathfinding;
using UnityEngine;

public class ErosionSolver {
	bool ErodeNodesAtBoundaries     { get; }
	int  NodesToErodeAtBoundaries   { get; }
	int  StartingNodeIndexToErode   { get; }
	int  NumberOfErodedNodesPerTile { get; }
	int  NumberOfErosionIterations  { get; }
	
	public void Erode(GridGraph graph) {
		graph.erosionUseTags = ErodeNodesAtBoundaries;

		if (!ErodeNodesAtBoundaries)
			return;

		graph.erodeIterations = NodesToErodeAtBoundaries;
		graph.erosionFirstTag = StartingNodeIndexToErode;
	}

	public ErosionSolver(ProceduralConfig config) {
		NumberOfErodedNodesPerTile = Mathf.RoundToInt(1 / Mathf.Pow(config.NavGraphNodeSize, 2));
		NumberOfErosionIterations  = (int)Mathf.Sqrt(NumberOfErodedNodesPerTile);
		ErodeNodesAtBoundaries     = config.ErodePathfindingGrid;
		NodesToErodeAtBoundaries   = config.NodesToErodeAtBoundaries;
		StartingNodeIndexToErode   = config.StartingNodeIndexToErode;
	}
}