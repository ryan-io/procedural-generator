using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration.Gizmos {
	public class DrawBorder : MonoBehaviour {
		[field: SerializeField] public ProceduralGenerator Generator                { get; private set; }
		[field: SerializeField] public bool                DrawProcessed   { get; private set; }
		[field: SerializeField] public bool                DrawUnprocessed    { get; private set; }

		
		void OnDrawGizmos() {
			if (Generator == null || (!DrawProcessed && !DrawUnprocessed))
				return;

			Coordinates coordinates = Generator.GeneratedCoordinates;

			if (DrawProcessed && !coordinates.ProcessedCoords.IsEmptyOrNull()) {
				foreach (var coord in coordinates.ProcessedCoords.Values) {
					foreach (var position in coord) {
						DebugExt.DrawPoint(position, Color.red, 0.5f);
					}
				}
			}

			if (DrawUnprocessed && !coordinates.UnprocessedCoords.IsEmptyOrNull()) {
				foreach (var coord in coordinates.UnprocessedCoords.Values) {
					foreach (var position in coord) {
						DebugExt.DrawCircle(position, Color.magenta, true, 0.25f);
					}
				}
			}
		}
	}
}