using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BCL;
using Engine.Procedural.Runtime;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Engine.Procedural {
	public class SpriteShapeBorderSolver {
		GameObject                  Go                       { get; set; }
		SerializedBoundaryShape     Shape                    { get; set; }
		SerializedBoundaryPositions ShapePositions           { get; set; }
		SerializedBoundaryPositions SplinePositions          { get; set; }
		float                       LastSlope                { get; set; }
		bool                        IsFirstIterationComplete { get; set; }
		bool                        WasNotSetLast            { get; set; }

		bool TangentModeIsLinear => _config.SplineTangentMode == ShapeTangentMode.Linear;

		public void GenerateProceduralBorder(MapData data, GameObject owner, [CallerMemberName] string name = "") {
			SetupGameObject(owner, name);

			var outlinesIndex = 0;

			foreach (var outline in data.RoomOutlines) {
				Shape.Add(outlinesIndex, outline);
				outlinesIndex++;
			}

			RunProcedure(data.MeshVertices, true);
			SerializeData();
		}

		void RunProcedure(IReadOnlyList<Vector3> boundaryPositions, bool shouldAddToCollections) {
			GenLogging.Instance.Log(Message.START_SHAPE_BOUNDARY_GENERATION, Constants.SPRITE_BOUNDARY_CTX);
			ClearBorderGameObjects();

			var currentRoomIndex = 0;

			foreach (var outline in Shape) {
				CreateSpriteShapeBorderAndPopulate(
					outline.Value,
					boundaryPositions,
					currentRoomIndex,
					shouldAddToCollections);

				currentRoomIndex++;
			}
		}

		void CreateSpriteShapeBorderAndPopulate(
			IReadOnlyList<int> outline, IReadOnlyList<Vector3> boundaryPositions,
			int currentRoomIndex, bool shouldAddToCollections) {
			var currentIndexBoundaryPos = new List<Vector3>();

			CheckIfShouldCacheBoundaryPositions(currentRoomIndex, shouldAddToCollections, currentIndexBoundaryPos);

			// NEED TO CREATE PREFAB TO INSTANTIATIE -> CANNOT BE NULL
			var obj = InstantiateSpriteControllerPrefab();

			obj.name = Constants.SPRITE_BOUNDARY_KEY + currentRoomIndex;

			var controller = obj.GetComponent<SpriteShapeController>();
			controller.worldSpaceUVs = _config.IsSplineAdaptive;
			var spline = controller.spline;
			spline.Clear();

			var indexTracker = 0;
			var solver       = InstantiateTangentSolver();

			for (var i = 1; i < outline.Count; i++)
				indexTracker = CreateAndSetSplineSegment(
					outline, boundaryPositions, i, currentIndexBoundaryPos, spline, indexTracker, solver);

			CacheSplineSegmentPositions(spline, currentRoomIndex, shouldAddToCollections);
			controller.spline.isOpenEnded = false;
			controller.RefreshSpriteShape();
		}

		void CheckIfShouldCacheBoundaryPositions(int currentRoomIndex, bool shouldAddToCollections,
			List<Vector3> currentIndexBoundaryPos) {
			if (shouldAddToCollections)
				ShapePositions.Add(currentRoomIndex, currentIndexBoundaryPos);
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

		int CreateAndSetSplineSegment(IReadOnlyList<int> outline, IReadOnlyList<Vector3> boundaryPositions, int i,
			List<Vector3> currentIndexBoundaryPos, Spline spline, int indexTracker, ISplineSegmentSolver solver) {
			var position = GetNextPosition(outline, boundaryPositions, i) * GetScaleMod();
			currentIndexBoundaryPos.Add(position);

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

		// void DrawTangentCoords(Vector3 position, Spline spline, int indexTracker) {
		// 	if (_config.s) {
		// 		var o = Instantiate(TextMeshPrefab, gameObject.transform);
		// 		o.transform.position = position;
		// 		var textMesh = o.GetComponent<TextMeshPro>();
		// 		textMesh.text = GetCoordString(spline, indexTracker);
		// 	}
		// }

		// string GetCoordString(Spline spline, int index) {
		// 	var lT = spline.GetLeftTangent(index);
		// 	var rT = spline.GetRightTangent(index);
		//
		// 	var output = "Left(" + lT.x + ", " + lT.y + ") Right(" + rT.x + ", " + rT.y + ")";
		// 	return output;
		// }

		float GetScaleMod() => 1 / _config.ScaleModifier;

		static Vector3 GetNextPosition(IReadOnlyList<int> outline, IReadOnlyList<Vector3> boundaryPositions, int i) {
			var position = new Vector3(boundaryPositions[outline[i]].x, boundaryPositions[outline[i]].y, 0);
			return position;
		}

		void SetupGameObject(GameObject owner, string name) {
			Go = new GameObject(Constants.SPRITE_SHAPE_SAVE_PREFIX + name) {
				transform = { parent = owner.transform.parent }
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
				Shape,
				ShapePositions,
				SplinePositions);
		}

		public SpriteShapeBorderSolver(SpriteShapeConfig config, string name) {
			_config = config;

			Shape           = new SerializedBoundaryShape();
			ShapePositions  = new SerializedBoundaryPositions();
			SplinePositions = new SerializedBoundaryPositions();
		}

		readonly SpriteShapeConfig _config;
	}
}