// using Pathfinding;
// using Source;
//
// namespace Engine.Procedural {
// 	public class GraphScanFinalize {
// 		public void Finalize(GridGraph gridGraph, GraphScanFinalizeSettings settings) {
// 			gridGraph.collision.use2D                  = false;
// 			gridGraph.collision.heightCheck            = true;
// 			gridGraph.collision.unwalkableWhenNoGround = false;
// 			gridGraph.collision.type                   = settings.CollisionType;
// 			gridGraph.collision.heightMask             = settings.HeightTestLayerMask;
// 			gridGraph.collision.diameter               = settings.CollisionDetectionDiameter;
// 			gridGraph.collision.height                 = settings.CollisionDetectionHeight;
// 			gridGraph.collision.mask                   = settings.ObstacleLayerMask;
// 			var proxy = new InternalEventProxy();
// 			proxy.TriggerEvent(new GraphScanComplete());
// 		}
// 	}
// }