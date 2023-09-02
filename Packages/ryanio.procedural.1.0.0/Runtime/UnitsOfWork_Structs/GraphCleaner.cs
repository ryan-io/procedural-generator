// Engine.Procedural

namespace ProceduralGeneration {
	public readonly struct GraphCleaner {
		public void Clean() {
			AstarPath.FindAstarPath();
			
			if (!AstarPath.active)
				return;
			
			var data = AstarPath.active.data;

			if (data == null || data.graphs.Length < 1)
				return;

			var count = data.graphs.Length;

			for (var i = 0; i < count; i++) {
				if (data.graphs[i] == null)
					continue;

				data.RemoveGraph(data.graphs[i]);
			}
		}
	}
}