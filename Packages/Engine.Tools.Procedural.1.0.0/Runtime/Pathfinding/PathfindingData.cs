// using System.Collections.Generic;
// using UnityBCL;
// using UnityEngine;
//
// namespace Engine.Procedural {
// 	public class PathfindingData : IPathfindingData {
// 		readonly ProceduralPathfindingSolverMonobehaviorModel _model;
// 		readonly Vector3                                      _normal = new(0, 0, 1);
//
// 		public PathfindingData(ProceduralPathfindingSolverMonobehaviorModel model) => _model = model;
//
// 		public List<Vector3> NodePositions        { get; } = new();
// 		public List<Vector3> TilePositions        { get; } = new();
// 		public List<Vector3> TilePositionsShifted { get; } = new();
//
// 		public int NodesCheckedThatWereNotNull { get; set; }
// 		public int NodesCheckThatWereNull      { get; set; }
// 		public int NodesWithTile               { get; set; }
// 		public int NodesWithoutTile            { get; set; }
//
// 		public void DrawGizmos() {
// 			if (!Application.isPlaying)
// 				return;
//
// 			if (_model.DrawNodePositionGizmos)
// 				for (var i = 0; i < NodePositions.Count; i++)
// 					DebugExt.DrawCircle(NodePositions[i], _normal, Color.cyan, .1f);
//
// 			if (_model.DrawTilePositionGizmos)
// 				for (var i = 0; i < TilePositions.Count; i++)
// 					DebugExt.DrawCircle(TilePositions[i], _normal, Color.yellow, .1f);
//
// 			if (_model.DrawTilePositionShiftedGizmos)
// 				for (var i = 0; i < TilePositionsShifted.Count; i++)
// 					DebugExt.DrawCircle(TilePositionsShifted[i], _normal, Color.white, .1f);
//
// 			if (_model.DrawTilePositionShiftedGizmos)
// 				for (var i = 0; i < TilePositionsShifted.Count; i++)
// 					DebugExt.DrawCircle(TilePositionsShifted[i], _normal, Color.white, .1f);
// 		}
// 	}
// }