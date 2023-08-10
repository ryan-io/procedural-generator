using System;
using BCL;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralGeneration {
	public class GridGraphBuilder : NavGraphBuilder<GridGraph> {
		int2         RealMapDimensions         { get; }
		ColliderType ColliderType              { get; }
		float        NavGraphNodeSize          { get; }
		float        NavGraphCollisionDiameter { get; }
		float        CollisionDetectionHeight  { get; }
		LayerMask    ObstacleLayerMask         { get; }
		LayerMask    HeightLayerMask           { get; }

		public override GridGraph Build() {
			var astarData = new GetActiveAstarData().Retrieve();
			var graph     = astarData.AddGraph(typeof(GridGraph)) as GridGraph;

			if (graph == null) {
				var exception = new Exception(Message.CANNOT_CAST_GRAPH_ERROR);
				GenLogging.Instance.Log(exception.Message, "BuildGridGraph", LogLevel.Error);
			}

			graph.name = Constants.ASTAR_GRAPH_NAME;
			SetGraph(graph);

			return graph;
		}

		public void SetGraph(GridGraph graph) {
			graph.cutCorners = true;
			graph.is2D       = true;
			graph.rotation   = new Vector3(90f, 0, 0);

			graph.collision.heightCheck            = true;
			graph.collision.unwalkableWhenNoGround = false;
			graph.collision.type                   = ColliderType;
			graph.collision.heightMask             = HeightLayerMask;
			graph.collision.height                 = CollisionDetectionHeight;
			//TODO: I feel this may need to be set to false. There old generator had this property set to false.
			graph.collision.use2D    = true;
			graph.collision.diameter = NavGraphCollisionDiameter;
			graph.collision.mask     = ObstacleLayerMask;

			var graphDimensions = DetermineGraphDimensions();
			graph.SetDimensions(graphDimensions.x, graphDimensions.y, NavGraphNodeSize);
		}

		/// <summary>
		/// Calculates the appropriate NavGraph dimensions
		/// A* 2D is XZ
		/// </summary>
		/// <returns>int2 with 'X' & 'Z' dimensions</returns>
		int2 DetermineGraphDimensions() {
			//TODO: is this calculation correct? RealMapDimensions is created in the same manner
			// var targetWidth  = RealMapDimensions.x * Constants.CELL_SIZE;
			// var targetHeight = RealMapDimensions.y * Constants.CELL_SIZE;

			var sizeX = Mathf.RoundToInt(RealMapDimensions.x / NavGraphNodeSize);
			var sizeY = Mathf.RoundToInt(RealMapDimensions.y / NavGraphNodeSize);
			return new int2(sizeX, sizeY);
		}

		public GridGraphBuilder(ProceduralConfig config) {
			RealMapDimensions         = new MapDimensionsIncludeCellSize(config).Get();
			NavGraphNodeSize          = config.NavGraphNodeSize;
			NavGraphCollisionDiameter = config.NavGraphCollisionDetectionDiameter;
			ObstacleLayerMask         = config.ObstacleLayerMask;
			ColliderType              = config.NavGraphCollisionType;
		}
	}
}