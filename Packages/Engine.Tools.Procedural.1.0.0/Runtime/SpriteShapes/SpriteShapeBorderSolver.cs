using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BCL;
using Engine.Procedural.Runtime;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Engine.Procedural {
	public class SpriteShapeBorderSolver {
		Transform                   Owner                    { get; }
		GameObject                  Go                       { get; set; }
		SerializedBoundaryPositions SplinePositions          { get; set; }
		float                       LastSlope                { get; set; }
		bool                        IsFirstIterationComplete { get; set; }
		bool                        WasNotSetLast            { get; set; }

		bool TangentModeIsLinear => _config.SplineTangentMode == ShapeTangentMode.Linear;

		public void GenerateProceduralBorder(MapData data, string serializedName,
			[CallerMemberName] string name = "") {
			SetupGameObject(serializedName);
			var dict = data.BoundaryCorners;

			RunProcedure(dict);
			SerializeData();
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
			controller.fillPixelsPerUnit  = 16;
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
					var newObjName = i + Constants.SPACE + iterationCount;
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

			}

			//CacheSplineSegmentPositions(spline, currentRoomIndex);
			controller.RefreshSpriteShape();
		}

		// void CheckIfShouldCacheBoundaryPositions(int currentRoomIndex, bool shouldAddToCollections,
		// 	List<Vector3> currentIndexBoundaryPos) {
		// 	if (shouldAddToCollections)
		// 		ShapePositions.Add(currentRoomIndex, currentIndexBoundaryPos);
		// }

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
			//currentIndexBoundaryPos.Add(position);

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

		void CacheSplineSegmentPositions(Spline spline, int currentIndex, bool shouldAddToCollections) {
			var splineSegmentList = new List<Vector3>();

			if (shouldAddToCollections)
				SplinePositions.Add(currentIndex, splineSegmentList);

			for (var i = 0; i < spline.GetPointCount(); i++) splineSegmentList.Add(spline.GetPosition(i));
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

		void SerializeData() {
			_config.SerializedData = new BorderShapeData(
				SplinePositions);
		}

		public SpriteShapeBorderSolver(SpriteShapeConfig config, GameObject owner) {
			_config         = config;
			Owner           = owner.transform;
			SplinePositions = new SerializedBoundaryPositions();
		}

		readonly SpriteShapeConfig _config;
	}
}