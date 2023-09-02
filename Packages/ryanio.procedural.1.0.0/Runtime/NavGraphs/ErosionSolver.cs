// Engine.Procedural

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
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

			// try {
			// 	ProcessErosion(graph);
			// 	GenLogging.Instance.Log("Counting nodes passed: " + graph.CountNodes(), "AnotherNodeCountVeri.");
			// 	GenLogging.Instance.Log("Total shifted tile positions is: " + TilePositionsShifted.Count,
			// 		"TilePositions");
			// }
			// catch (Exception e) {
			// 	GenLogging.Instance.Log(e.Message, e.GetMethodThatThrew(out _), LogLevel.Error);
			// }

			return new ErosionSolverData(NodePositions, TilePositions, TilePositionsShifted);
		}

		unsafe void ProcessErosion(GridGraph graph, [CallerMemberName] string caller = "") {
			// may need to use Astar.Active.gridGraph
			// grab the node positions within from the active graph. do not try to create a vector in a roundabout way.
			// graph node positions are constructed assuming x-z coords. y-axis is for height testing.
			var bounds = BoundaryTilemap.cellBounds;
			graph.GetNodes(node => NodePositions.Add((Vector3)node.position));

			var  array   = NodePositions.ToArray();
			int* pointer = stackalloc int[3*array.Length];
			var  span    = new Span<Vector3>(pointer, array.Length);

			for (var i = 0; i < span.Length; i++) {
				span[i] = array[i];
			}

			for (var i = 0; i < span.Length; i++)
				QueryGridDataForNodes(span[i], bounds, graph);
		}

		void QueryGridDataForNodes(Vector3 position, BoundsInt bounds, NavGraph gridGraph) {
			var nodePositionPrime = new Vector3Int(
				Mathf.FloorToInt(position.x) + bounds.xMax / 2,
				Mathf.FloorToInt(position.y) + bounds.yMax / 2,
				Mathf.FloorToInt(position.z) + bounds.zMax / 2);

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
				LocalFloodFill(NumberOfErosionIterations, gridGraph, position);
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

		public ErosionSolver(ProceduralConfig config, TileHashset tileHashset, TileMapDictionary dictionary) {
			NumberOfErodedNodesPerTile = Mathf.RoundToInt(1 / Mathf.Pow(config.NavGraphNodeSize, 2));
			NumberOfErosionIterations  = (int)Mathf.Sqrt(NumberOfErodedNodesPerTile);
			ErodeNodesAtBoundaries     = config.ErodePathfindingGrid;
			NodesToErodeAtBoundaries   = config.NodesToErodeAtBoundaries;
			StartingNodeIndexToErode   = config.StartingNodeIndexToErode;
			BoundaryTilemap            = dictionary[TileMapType.Boundary];
			GroundTilemap              = dictionary[TileMapType.Ground];
			ErosionGridNodeSize        = config.NavGraphNodeSize;

			NodePositions        = new List<Vector3>();
			TilePositions        = new List<Vector3>();
			TilePositionsShifted = new List<Vector3>();

			TileHashset = tileHashset;
		}
	}
}