using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	internal class SpriteShapeBorderSolver {
		const Ppu                               PPU = Ppu.Eight;
		Transform                               Owner          { get; }
		GameObject                              Go             { get; set; }
		IReadOnlyDictionary<int, List<Vector3>> Coordinates    { get; }
		string                                  SerializedName { get; }

		internal void Generate([CallerMemberName] string name = "") {
			SetupGameObject();
			RunProcedure();
			UpdateAABB();
		}

		void RunProcedure() {
			ClearBorderGameObjects();

			var currentRoomIndex = 0;

			foreach (var outline in Coordinates) {
				CreateSpriteShapeBorderAndPopulate(outline.Value, currentRoomIndex);
				currentRoomIndex++;
			}
		}

		void CreateSpriteShapeBorderAndPopulate(IReadOnlyList<Vector3> boundaryPositions, int currentRoomIndex) {
			var obj = InstantiateSpriteControllerPrefab();
			obj.name = Constants.SPRITE_BOUNDARY_KEY + currentRoomIndex;

			var controller = obj.GetComponent<SpriteShapeController>();
			controller.worldSpaceUVs     = _config.IsSplineAdaptive;
			controller.fillPixelsPerUnit = (int)PPU;

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
						currentRoomIndex              +
						Constants.SPACE               +
						Constants.ITERATION_LABEL     +
						iterationCount;

					obj      = InstantiateSpriteControllerPrefab();
					obj.name = newObjName;

					controller               = obj.GetComponent<SpriteShapeController>();
					controller.worldSpaceUVs = _config.IsSplineAdaptive;

					spline = controller.spline;
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

		void UpdateAABB() {
			var objs = Go.GetComponentsInChildren<Transform>();

			if (objs.IsEmptyOrNull()) {
				return;
			}

			foreach (var tr in objs) {
				if (!tr)
					continue;
				
				var controller = tr.GetComponent<SpriteShapeController>();
				var rend       = tr.GetComponent<SpriteShapeRenderer>();

				if (!controller || !rend)
					continue;
				
				UpdateLocalAABB.Update(rend, controller, tr);
			}
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

		void SetupGameObject() {
			Go = new GameObject(Constants.SPRITE_SHAPE_SAVE_PREFIX + SerializedName) {
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
			_config        = ctx.Config;
			Owner          = ctx.Owner.transform;
			Coordinates    = ctx.Coordinates;
			SerializedName = ctx.SerializedName;
		}

		readonly SpriteShapeConfig _config;
	}
}