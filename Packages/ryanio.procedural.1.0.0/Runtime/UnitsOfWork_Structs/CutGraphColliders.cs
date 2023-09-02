using System.Collections.Generic;
using UnityBCL;

namespace ProceduralGeneration {
	/// <summary>
	/// Simple struct that take a readonly collection of type GraphColliderCutter and invokes the Cut() method
	/// Graph collider cutters should contain a collider and a GraphColliderCutter monobehavior
	/// Once a NavGraph has been generated and scanned, run this unit of work to remove any NavGraph nodes that
	///		have a collider & GraphColliderCutter components
	/// </summary>
	public readonly struct CutGraphColliders {
		public void Cut(IReadOnlyList<GraphColliderCutter> graphCutters) {
			if (graphCutters.IsEmptyOrNull())
				return;

			foreach (var cutter in graphCutters) {
				if (cutter)
					cutter.Cut();
			}
		}
	}
}