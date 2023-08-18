using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	internal class SpriteShapeBorderSolver {
		Ppu        Ppu   { get; }
		Transform  Owner { get; }
		GameObject Go    { get; set; }

		internal void GenerateProceduralBorder(
			Dictionary<int, List<Vector3>> positions, 
			string serializedName,
			[CallerMemberName] string name = "") {
			SetupGameObject(serializedName);
			RunProcedure(positions);
		}

		void RunProcedure(Dictionary<int, List<Vector3>> boundaryPositions) {
			GenLogging.Instance.Log(Message.START_SHAPE_BOUNDARY_GENERATION, Constants.SPRITE_BOUNDARY_CTX);
			ClearBorderGameObjects();

			var currentRoomIndex = 0;

			foreach (var outline in boundaryPositions) {
				CreateSpriteShapeBorderAndPopulate(outline.Value, currentRoomIndex);
				currentRoomIndex++;
			}
		}

		
		void CreateSpriteShapeBorderAndPopulate(IReadOnlyList<Vector3> boundaryPositions, int currentRoomIndex) {
			var obj = InstantiateSpriteControllerPrefab();
			obj.name = Constants.SPRITE_BOUNDARY_KEY + currentRoomIndex;

			var controller = obj.GetComponent<SpriteShapeController>();
			controller.worldSpaceUVs      = _config.IsSplineAdaptive;
			controller.fillPixelsPerUnit  = (int)Ppu;
			var spline = controller.spline;
			controller.fillPixelsPerUnit = (float)_config.Ppu;
			spline.Clear();
			controller.spline.isOpenEnded = true;

			var indexTracker   = 0;
			var solver         = InstantiateTangentSolver();
			var iterationCount = 2;

			int maxNodes = 250;
			var limit    = boundaryPositions.Count;

			for (var i = 0; i < limit; i++) {
				if (spline.GetPointCount() > maxNodes) {
					var newObjName = 
						Constants.SPRITE_BOUNDARY_KEY + 
						currentRoomIndex + 
						Constants.SPACE + 
						Constants.ITERATION_LABEL + 
						iterationCount;
					
					obj      = InstantiateSpriteControllerPrefab();
					obj.name = newObjName;

					controller                    = obj.GetComponent<SpriteShapeController>();
					controller.worldSpaceUVs      = _config.IsSplineAdaptive;
					
					spline                        = controller.spline;
					spline.Clear();
					
					iterationCount++;
					indexTracker = 0;
					
					controller.spline.isOpenEnded = true;
					controller.fillPixelsPerUnit  = (float)_config.Ppu;
					controller.RefreshSpriteShape();
				}
				
				indexTracker = CreateAndSetSplineSegment(boundaryPositions[i], spline, indexTracker, solver);
				controller.RefreshSpriteShape();
			}

		}
		ISplineSegmentSolver InstantiateTangentSolver() {
			ISplineSegmentSolver solver;
			switch (_config.SplineTangentMode) {
				case ShapeTangentMode.Linear:
					solver = new LinearSplineSegmentSolver(_config);
					break;
				case ShapeTangentMode.Continuous:
					solver = new ContinuousSplineSegmentSolver();
					break;
				case ShapeTangentMode.Broken:
				default:
					solver = new LinearSplineSegmentSolver(_config);
					break;
			}

			return solver;
		}

		int CreateAndSetSplineSegment(Vector3 position, Spline spline, int indexTracker, ISplineSegmentSolver solver) {
			if (solver.DetermineNextSegment(spline, position, indexTracker))
				indexTracker++;

			return indexTracker;
		}

		void ClearBorderGameObjects() {
			var borderObjects = Go.GetComponentsInChildren<Transform>();

			foreach (var borderObject in borderObjects) {
				if (borderObject == Go.transform)
					continue;

				if (Application.isPlaying)
					Object.Destroy(borderObject.gameObject);
				else
					Object.DestroyImmediate(borderObject.gameObject);
			}
		}

		void SetupGameObject(string name) {
			Go = new GameObject(Constants.SPRITE_SHAPE_SAVE_PREFIX + name) {
				transform = { parent = Owner }
			};
		}

		GameObject InstantiateSpriteControllerPrefab() {
			var obj = Object.Instantiate(_config.ControllerPrefab, Go.transform);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale    = _config.ScaleModifier * obj.transform.localScale;
			return obj;
		}

		internal SpriteShapeBorderSolver(SpriteShapeBorderCtx ctx) {
			_config         = ctx.Config;
			Owner           = ctx.Owner.transform;
		}

		readonly SpriteShapeConfig _config;
	}
}