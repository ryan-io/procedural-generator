using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	internal class SpriteShapeBorderSolver {
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
				if (outline.Value.Count <= CUT_OFF)
					continue;

				CreateSpriteShapeBorderAndPopulate(outline.Value, currentRoomIndex);
				currentRoomIndex++;
			}
		}

		void CreateSpriteShapeBorderAndPopulate(IReadOnlyList<Vector3> boundaryPositions, int currentRoomIndex) {
			const int meshNodeLimit    = 100;
			const int minimumNodeCount = 20;
			var       forcePass        = true;

			var solver         = InstantiateTangentSolver();
			var ctx            = new SpriteShapeObjectCtx(default, default);
			var nodeTracker    = int.MaxValue;
			var limit          = boundaryPositions.Count;
			var limitTracker   = 0;
			var iterationCount = 0;

			while (true) {
				if (nodeTracker > meshNodeLimit && (forcePass || limit - nodeTracker >= minimumNodeCount)) {
					forcePass = false;
					iterationCount++;
					var name = GetName(currentRoomIndex, iterationCount);

					ctx = SetupNewSpriteShape(name);
					ctx.Controller.spline.Clear();
					ctx.Controller.UpdateSpriteShapeParameters();
					ctx.Controller.RefreshSpriteShape();

					limitTracker--;
					limitTracker = Math.Clamp(limitTracker, 0, limit);
					nodeTracker = 0;
				}

				if (limitTracker >= limit)
					break;

				nodeTracker = CreateAndSetSplineSegment(
					boundaryPositions[limitTracker],
					ctx.Controller.spline,
					nodeTracker,
					solver);

				limitTracker++;
			}

			//for (var i = 0; i < limit; i++) {
			// if (ctx.Controller.spline.GetPointCount() > maxNodes) {
			// 	ctx.Controller.UpdateSpriteShapeParameters();
			// 	ctx.Controller.RefreshSpriteShape();
			//
			// 	iterationCount++;
			// 	ctx = SetupNewSpriteShape(GetName(currentRoomIndex, iterationCount));  
			//
			// 	i = 0;
			// }

			// nodeTracker =
			// 	CreateAndSetSplineSegment(boundaryPositions[i], ctx.Controller.spline, nodeTracker, solver);
			//}

			// ctx.Controller.UpdateSpriteShapeParameters();
			// ctx.Controller.RefreshSpriteShape();
		}

		static string GetName(int currentRoomIndex, int iterationCount) {
			return
				Constants.SPRITE_BOUNDARY_KEY +
				currentRoomIndex              +
				Constants.SPACE               +
				Constants.ITERATION_LABEL     +
				iterationCount;
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

		SpriteShapeObjectCtx SetupNewSpriteShape(string name) {
			var obj = new GameObject {
				transform = {
					localPosition = Vector3.zero,
					parent        = Go.transform
				},
				name = name
			};

			obj.transform.localScale = _config.ScaleModifier * obj.transform.localScale;

			var controller = obj.AddComponent<SpriteShapeController>();

			ParameterizeController(controller);
			ParameterizeRenderer(controller.spriteShapeRenderer);

			controller.spline.Clear();
			controller.UpdateSpriteShapeParameters();
			controller.RefreshSpriteShape();

			return new SpriteShapeObjectCtx(obj, controller);
		}

		void ParameterizeController(SpriteShapeController controller) {
			controller.spriteShape          = _config.Profile;
			controller.spline.isOpenEnded   = _config.IsOpenEnded;
			controller.worldSpaceUVs        = false;
			controller.fillPixelsPerUnit    = (float)_config.Ppu;
			controller.cornerAngleThreshold = _config.CornerThreshold;
			controller.stretchTiling        = 0.0f;
			controller.enableTangents       = _config.EnableTangents;
			controller.autoUpdateCollider   = true;

			const float initScalar = 2.0f;

			for (var i = 0; i < 4; i++) {
				controller.spline.InsertPointAt(i, new Vector3(i * initScalar, i * -initScalar));
			}
		}

		void ParameterizeRenderer(SpriteShapeRenderer renderer) {
			var materials = renderer.sharedMaterials;

			materials[0] = _config.FillMaterial;
			materials[1] = _config.EdgeMaterial;

			renderer.sortingLayerName = Constants.SortingLayer.OBSTACLES;
			renderer.sortingOrder     = _config.OrderInLayer;
		}

		internal SpriteShapeBorderSolver(SpriteShapeBorderCtx ctx) {
			_config        = ctx.Config;
			Owner          = ctx.Owner.transform;
			Coordinates    = ctx.Coordinates;
			SerializedName = ctx.SerializedName;
		}

		readonly SpriteShapeConfig _config;
		const    int               CUT_OFF = 10;
	}
}