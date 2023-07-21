// Engine.Procedural

using System;
using System.Collections.Generic;
using System.Diagnostics;
using BCL;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace Engine.Procedural {
	public class ErosionSolver {
		List<Vector3> NodePositions        { get; }
		List<Vector3> TilePositions        { get; }
		List<Vector3> TilePositionsShifted { get; }

		TileHashset TileHashset                { get; }
		Tilemap     BoundaryTilemap            { get; }
		Tilemap     GroundTilemap              { get; }
		float       ErosionGridNodeSize        { get; }
		bool        ErodeNodesAtBoundaries     { get; }
		int         NodesToErodeAtBoundaries   { get; }
		int         StartingNodeIndexToErode   { get; }
		int         NumberOfErodedNodesPerTile { get; }
		int         NumberOfErosionIterations  { get; }

		int NodesCheckedThatWereNotNull { get; set; }
		int NodesCheckThatWereNull      { get; set; }
		int NodesWithTile               { get; set; }
		int NodesWithoutTile            { get; set; }

		public ErosionSolverData Erode(GridGraph graph) {
			graph.erosionUseTags  = ErodeNodesAtBoundaries;
			graph.erodeIterations = NodesToErodeAtBoundaries;
			graph.erosionFirstTag = StartingNodeIndexToErode;

			ProcessErosion(graph);

			GenLogging.Instance.Log("Counting nodes passed: " + graph.CountNodes(), "AnotherNodeCountVeri.");
			GenLogging.Instance.Log("Total shifted tile positions is: " + TilePositionsShifted.Count, "TilePositions");

			return new ErosionSolverData(NodePositions, TilePositions, TilePositionsShifted);
		}

		void ProcessErosion(GridGraph graph) {
			// may need to use Atar.Active.gridGraph
			// grab the node positions within from the active graph. do not try to create a vector in a roundabout way.
			// graph node positions are constructed assuming x-z coords. y-axis is for height testing.
			var bounds = BoundaryTilemap.cellBounds;
			//monoModel.ErosionIterator = (int)Mathf.Sqrt(monoModel.NumberOfErodedNodesPerTile);
			graph.GetNodes(node => NodePositions.Add((Vector3)node.position));

			// int* pointer  = stackalloc int[NodePositions.Count];
			// var  span     = new Span<Vector3>(pointer, NodePositions.Count);
			var span = new Span<Vector3>(NodePositions.ToArray());

			for (var i = 0; i < span.Length; i++)
				QueryGridDataForNodes(i, bounds, graph);
		}

		void QueryGridDataForNodes(int i, BoundsInt bounds, NavGraph gridGraph) {
			var nodePosition = NodePositions[i];
			var nodePositionPrime = new Vector3Int(
				Mathf.FloorToInt(nodePosition.x) + bounds.xMax / 2,
				Mathf.FloorToInt(nodePosition.y) + bounds.yMax / 2,
				Mathf.FloorToInt(nodePosition.z) + bounds.zMax / 2);

			var cellPositionPrime = GroundTilemap.GetCellCenterWorld(nodePositionPrime);
			var cellPosition2D    = new Vector2Int(nodePositionPrime.x, nodePositionPrime.y);
			var tileData          = TileHashset[cellPosition2D];

			TilePositions.Add(cellPositionPrime);
			var shifted = ShiftPosition0Z(cellPositionPrime, ErosionGridNodeSize / 2f);
			TilePositionsShifted.Add(shifted);

			var node    = gridGraph.GetNearest(cellPositionPrime).node;
			var hasTile = GroundTilemap.HasTile(nodePositionPrime);

			if (hasTile && !tileData.IsMapBoundary && !tileData.IsLocalBoundary) {
				if (node != null)
					NodesCheckedThatWereNotNull++;
				else
					NodesCheckThatWereNull++;

				NodesWithTile++;
			}
			else {
				LocalFloodFill(NumberOfErosionIterations, gridGraph, nodePosition);
				NodesWithoutTile++;
			}
		}

		void LocalFloodFill(int iterator, NavGraph gridGraph, Vector3 shiftPosition) {
			for (var x = 0; x < iterator; x++) {
				for (var y = 0; y < iterator; y++) {
					var iteratorNode = gridGraph.GetNearest(shiftPosition).node;
					AstarPath.active.AddWorkItem(() => iteratorNode.Walkable = false);
					shiftPosition = new Vector3(shiftPosition.x, shiftPosition.y + ErosionGridNodeSize, 0);
				}

				shiftPosition = new Vector3(shiftPosition.x + ErosionGridNodeSize, shiftPosition.y, 0);
			}
		}

		Vector3 ShiftPosition0Z(Vector3 original, float shiftValue)
			=> new(original.x + shiftValue, original.y + shiftValue, 0f);

		public ErosionSolver(ProceduralConfig config, TileHashset tileHashset) {
			NumberOfErodedNodesPerTile = Mathf.RoundToInt(1 / Mathf.Pow(config.NavGraphNodeSize, 2));
			NumberOfErosionIterations  = (int)Mathf.Sqrt(NumberOfErodedNodesPerTile);
			ErodeNodesAtBoundaries     = config.ErodePathfindingGrid;
			NodesToErodeAtBoundaries   = config.NodesToErodeAtBoundaries;
			StartingNodeIndexToErode   = config.StartingNodeIndexToErode;
			BoundaryTilemap            = config.TileMapDictionary[TileMapType.Boundary];
			GroundTilemap              = config.TileMapDictionary[TileMapType.Ground];
			ErosionGridNodeSize        = config.NavGraphNodeSize;

			NodePositions        = new List<Vector3>();
			TilePositions        = new List<Vector3>();
			TilePositionsShifted = new List<Vector3>();

			TileHashset = tileHashset;
		}
	}
}