// ProceduralGeneration

using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration.Gizmos {
	public class ProceduralDebuggingTools : MonoBehaviour {
		[SerializeField] bool _drawMapPosOnGrid = false;

		[SerializeField, ShowIf("@_drawMapPosOnGrid")]
		Grid _grid;

		[ShowInInspector, ReadOnly] float CellSize => Constants.Instance.CellSize;

		Vector3[,] _positions;

		bool notSet;

		[Button]
		void ResetPositions() {
			notSet = false;
			_positions = null;
		}

		//public float RunThrough();;
		
		void OnDrawGizmosSelected() {
			if (_drawMapPosOnGrid && Generator.Data != null && _grid) {
				var map = Generator.Data.Map;

				if (!notSet) {
					var dims = Generator.Actions.GetMapDimensions();
					_positions = new Vector3[map.GetLength(0), map.GetLength(1)];

					for (var x = 0; x < map.GetLength(0); x++) {
						for (var y = 0; y < map.GetLength(1); y++) {
							var offsetX = Constants.Instance.CellSize / 2f - (Constants.Instance.CellSize * dims.Rows    / 2f);
							var offsetY = Constants.Instance.CellSize / 2f - (Constants.Instance.CellSize * dims.Columns / 2f);

							// _offsetX = Constants.Instance.CellSize / 2f - Constants.Instance.CellSize * dims.Rows    / 2f;
							// _offsetY = Constants.Instance.CellSize / 2f - Constants.Instance.CellSize * dims.Columns / 2f;
							
							_positions[x, y] = new Vector3(
								Constants.Instance.CellSize * x + offsetX,
								Constants.Instance.CellSize * y + offsetY,
								0);
						}

						notSet = true;
					}
				}

				for (var i = 0; i < map.GetLength(0); i++) {
					for (var j = 0; j < map.GetLength(1); j++) {
						Color color = map[i, j] == 1 ? Color.green : Color.red;
						DebugExt.DrawCircle(_positions[i, j], color, true, 0.5f);
					}
				}
			}
		}

		[Button]
		void ZoomSceneCamera(float size) {
			Temporary.ZoomSceneCamera(size);
		}

		[Button]
		void ValidateRendererIsVisible() {
			if (!Generator)
				return;

			var renderers = Generator.GetComponentsInChildren<SpriteShapeRenderer>();

			if (renderers.IsEmptyOrNull())
				return;

			var logger = new UnityLogging();

			foreach (var rend in renderers) {
				if (!rend)
					continue;

				logger.Log(rend.name + "visibility is " + rend.isVisible);
			}
		}

		[field: SerializeField, Required] public ProceduralGenerator Generator { get; private set; }
	}
}