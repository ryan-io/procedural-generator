// using Pathfinding;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Tilemaps;
//
// namespace Engine.Procedural.Editor.Editor {
// 	[CustomGridGraphRuleEditor(typeof(WalkabilityRule), "Procedural Walkability Rule")]
// 	public class WalkabilityNodeEditor : IGridGraphRuleEditor {
// 		SerializedObject _boundaryTilemap;
// 		SerializedObject _groundTilemap;
//
// 		public void OnInspectorGUI(GridGraph graph, GridGraphRule rule) {
// 			var ruleCast = rule as WalkabilityRule;
//
// 			ruleCast.BoundaryTilemap = DrawTileMap(ruleCast.BoundaryTilemap, "Boundary Tilemap");
// 			ruleCast.GroundTilemap   = DrawTileMap(ruleCast.GroundTilemap,   "Ground Tilemap");
// 		}
//
// 		public void OnSceneGUI(GridGraph graph, GridGraphRule rule) {
// 		}
//
// 		static Tilemap DrawTileMap(Object reference, string inspName)
// 			=> GraphEditor.ObjectField(
// 					   new GUIContent(inspName),
// 					   reference,
// 					   typeof(Tilemap),
// 					   true,
// 					   false)
// 				   as Tilemap;
// 	}
// }