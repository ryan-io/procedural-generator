using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration.Gizmos {
	public class DrawBorder : MonoBehaviour {
		[field: SerializeField] public ProceduralGenerator Generator        { get; private set; }
		[field: SerializeField] public bool                DrawProcessed    { get; private set; }
		[field: SerializeField] public bool                DrawUnprocessed  { get; private set; }
		[field: SerializeField] public bool                DrawRoomOutlines { get; private set; }

		void OnDrawGizmos() {
			if (!Generator)
				return;

			if (DrawRoomOutlines) {
				DrawRooms();
			}

			Coordinates coordinates = Generator.GeneratedCoordinates;

			if (DrawProcessed && !coordinates.ProcessedCoords.IsEmptyOrNull()) {
				DrawProcessedCoords(coordinates);
			}

			if (DrawUnprocessed && !coordinates.UnprocessedCoords.IsEmptyOrNull()) {
				DrawUnprocessedCoords(coordinates);
			}
		}

		void DrawRooms() {
			var rooms = Generator.Rooms;

			if (!rooms.IsEmptyOrNull()) {
				foreach (var room in rooms) {
					foreach (var coord in room.EdgeTiles) {
						DebugExt.DrawPoint(coord.ToVector3(), Color.cyan, 0.5f);
					}
				}
			}
		}

		static void DrawProcessedCoords(Coordinates coordinates) {
			foreach (var coord in coordinates.ProcessedCoords.Values) {
				foreach (var position in coord) {
					DebugExt.DrawPoint(position, Color.red, 0.5f);
				}
			}
		}

		static void DrawUnprocessedCoords(Coordinates coordinates) {
			foreach (var coord in coordinates.UnprocessedCoords.Values) {
				foreach (var position in coord) {
					DebugExt.DrawCircle(position, Color.magenta, true, 0.25f);
				}
			}
		}
	}
}