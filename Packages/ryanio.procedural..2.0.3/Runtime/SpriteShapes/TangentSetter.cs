// Engine.Procedural

using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	public class TangentSetter {
		const float TANGENT_SCALE = 0.05f;

		public void SetTangentDefaultZeroVector(Spline spline, int indexTracker) {
			spline.SetRightTangent(indexTracker, Vector3.zero);
			spline.SetLeftTangent(indexTracker, Vector3.zero);
		}

		public void SetTangentDefaultUnitVector(Spline spline, int indexTracker) {
			spline.SetRightTangent(indexTracker, new Vector3(TANGENT_SCALE, TANGENT_SCALE,  0));
			spline.SetLeftTangent(indexTracker, new Vector3(-TANGENT_SCALE, -TANGENT_SCALE, 0));
		}

		public void SetTangentsQuad14(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(-TANGENT_SCALE, 0, 0));
			spline.SetRightTangent(indexTracker, new Vector3(TANGENT_SCALE, 0, 0));
		}

		public void SetTangentsQuad1(Spline spline, int indexTracker) {
			spline.SetRightTangent(indexTracker, new Vector3(TANGENT_SCALE, TANGENT_SCALE,  0));
			spline.SetLeftTangent(indexTracker, new Vector3(-TANGENT_SCALE, -TANGENT_SCALE, 0));
		}

		public void SetTangentsQuad12(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(0,  -TANGENT_SCALE, 0));
			spline.SetRightTangent(indexTracker, new Vector3(0, TANGENT_SCALE,  0));
		}

		public void SetTangentsQuad23(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(TANGENT_SCALE,   0, 0));
			spline.SetRightTangent(indexTracker, new Vector3(-TANGENT_SCALE, 0, 0));
		}

		public void SetTangentsQuad2(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(TANGENT_SCALE,   -TANGENT_SCALE, 0));
			spline.SetRightTangent(indexTracker, new Vector3(-TANGENT_SCALE, TANGENT_SCALE,  0));
		}

		public void SetTangentsQuad3(Spline spline, int indexTracker) {
			spline.SetRightTangent(indexTracker, new Vector3(-TANGENT_SCALE, -TANGENT_SCALE, 0));
			spline.SetLeftTangent(indexTracker, new Vector3(TANGENT_SCALE,   TANGENT_SCALE,  0));
		}

		public void SetTangentsQuad34(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(0,  TANGENT_SCALE,  0));
			spline.SetRightTangent(indexTracker, new Vector3(0, -TANGENT_SCALE, 0));
		}

		public void SetTangentsQuad4(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(-TANGENT_SCALE, TANGENT_SCALE,  0));
			spline.SetRightTangent(indexTracker, new Vector3(TANGENT_SCALE, -TANGENT_SCALE, 0));
		}
	}
}