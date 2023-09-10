// ProceduralGeneration

using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration.Gizmos {
	public class ProceduralDebuggingTools : MonoBehaviour {
		[SerializeField] bool _drawMapPosOnGrid = false;
		[SerializeField, ShowIf("@_drawMapPosOnGrid")] Grid _grid;

		[ShowInInspector, ReadOnly] float CellSize => Constants.Instance.CellSize;

		void OnDrawGizmosSelected() {
			if (_drawMapPosOnGrid && Generator.Data!=null && _grid) {
				var map = Generator.Data.Map;
				
				for (var x = 0; x < map.GetLength(0); x++) {
					for (var y = 0; y < map.GetLength(1); y++) {
						var pos       = _grid.CellToWorld(new Vector3Int(x, y, 0));
						var offset    = Constants.Instance.CellSize /2f;
						
						Color color = map[x, y] == 1 ? Color.green : Color.red;
						
						DebugExt.DrawCircle(new Vector3(Constants.Instance.CellSize * x + offset, 
							Constants.Instance.CellSize * y + offset, 0), color, true, 0.5f);
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