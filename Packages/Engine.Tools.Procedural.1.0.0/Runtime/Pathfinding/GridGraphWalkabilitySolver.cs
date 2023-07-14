//TODO: What was the intent for this class? It is functional in testing workshop, but need to debug to determine how
// the method works
 // using Cysharp.Threading.Tasks;
// using Pathfinding;
// using UnityEngine;
//
// namespace Engine.Procedural {
// 	public static class GridGraphWalkabilitySolver {
// 		public static async UniTask DetermineWalkability(GridGraph graph, PathfindingData pathfindingData,
// 			PathfindingProgressData progressData, ProceduralPathfindingSolverMonobehaviorModel monoModel) {
// 			await UniTask.Yield();
// 			// may need to use Atar.Active.gridGraph
// 			// grab the node positions within from the active graph. do not try to create a vector in a roundabout way.
// 			// graph node positions are constructed assuming x-z coords. y-axis is for height testing.
// 			var bounds = progressData.BoundaryTilemap.cellBounds;
// 			//monoModel.ErosionIterator = (int)Mathf.Sqrt(monoModel.NumberOfErodedNodesPerTile);
// 			graph.GetNodes(node => pathfindingData.NodePositions.Add((Vector3)node.position));
//
// 			for (var i = 0; i < pathfindingData.NodePositions.Count; i++)
// 				await QueryGridDataForNodes(i, bounds, graph, pathfindingData, progressData, monoModel);
// 		}
//
// 		static UniTask QueryGridDataForNodes(int i, BoundsInt bounds, GridGraph gridGraph,
// 			PathfindingData pathfindingData, PathfindingProgressData progressData,
// 			ProceduralPathfindingSolverMonobehaviorModel monoModel) {
// 			var nodePosition = pathfindingData.NodePositions[i];
//
// 			var nodePositionPrime = new Vector3Int(
// 				Mathf.FloorToInt(nodePosition.x) + bounds.xMax / 2,
// 				Mathf.FloorToInt(nodePosition.y) + bounds.yMax / 2,
// 				Mathf.FloorToInt(nodePosition.z) + bounds.zMax / 2);
//
// 			var cellPositionPrime = progressData.GroundTilemap.GetCellCenterWorld(nodePositionPrime);
// 			var cellPosition2D    = new Vector2Int(nodePositionPrime.x, nodePositionPrime.y);
//
// 			// TODO - this will need to be verified. uncertain if this data structure is correct
// 			//var data = progressData.TileHashset.FirstOrDefault(t => t.Location == cellPosition2D);
// 			var tileData = progressData.TileHashset[cellPosition2D];
//
// 			pathfindingData.TilePositions.Add(cellPositionPrime);
// 			var shifted = ShiftPosition0Z(cellPositionPrime, monoModel.ErosionGridNodeSize / 2f);
// 			pathfindingData.TilePositionsShifted.Add(shifted);
//
// 			var node    = gridGraph.GetNearest(cellPositionPrime).node;
// 			var hasTile = progressData.GroundTilemap.HasTile(nodePositionPrime);
//
// 			if (hasTile && !tileData.IsMapBoundary && !tileData.IsLocalBoundary) {
// 				if (node != null)
// 					pathfindingData.NodesCheckedThatWereNotNull++;
// 				else
// 					pathfindingData.NodesCheckThatWereNull++;
//
// 				pathfindingData.NodesWithTile++;
// 			}
// 			else {
// 				LocalFloodFill(monoModel.ErosionIterator, gridGraph, nodePosition, monoModel);
// 				pathfindingData.NodesWithoutTile++;
// 			}
//
// 			return new UniTask();
// 		}
//
// 		static void LocalFloodFill(int iterator, GridGraph gridGraph, Vector3 shiftPosition,
// 			ProceduralPathfindingSolverMonobehaviorModel monoModel) {
// 			for (var x = 0; x < iterator; x++) {
// 				for (var y = 0; y < iterator; y++) {
// 					var iteratorNode = gridGraph.GetNearest(shiftPosition).node;
// 					AstarPath.active.AddWorkItem(() => iteratorNode.Walkable = false);
// 					shiftPosition = new Vector3(shiftPosition.x, shiftPosition.y + monoModel.ErosionGridNodeSize, 0);
// 				}
//
// 				shiftPosition = new Vector3(shiftPosition.x + monoModel.ErosionGridNodeSize, shiftPosition.y, 0);
// 			}
// 		}
//
// 		static Vector3 ShiftPosition0Z(Vector3 original, float shiftValue)
// 			=> new(original.x + shiftValue, original.y + shiftValue, 0f);
// 	}
// }