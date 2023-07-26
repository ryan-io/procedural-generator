// Engine.Procedural

using UnityEngine;
using UnityEngine.U2D;

namespace Engine.Procedural {
	public class TangentSetter {
		public void SetTangentDefaultZeroVector(Spline spline, int indexTracker) {
			spline.SetRightTangent(indexTracker, Vector3.zero);
			spline.SetLeftTangent(indexTracker, Vector3.zero);
		}

		public void SetTangentDefaultUnitVector(Spline spline, int indexTracker) {
			spline.SetRightTangent(indexTracker, new Vector3(1, 1,  0));
			spline.SetLeftTangent(indexTracker, new Vector3(-1, -1, 0));
		}

		public void SetTangentsQuad14(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(-1, 0, 0));
			spline.SetRightTangent(indexTracker, new Vector3(1, 0, 0));
		}

		public void SetTangentsQuad1(Spline spline, int indexTracker) {
			spline.SetRightTangent(indexTracker, new Vector3(1, 1,  0));
			spline.SetLeftTangent(indexTracker, new Vector3(-1, -1, 0));
		}

		public void SetTangentsQuad12(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(0,  -1, 0));
			spline.SetRightTangent(indexTracker, new Vector3(0, 1,  0));
		}

		public void SetTangentsQuad23(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(1,   0, 0));
			spline.SetRightTangent(indexTracker, new Vector3(-1, 0, 0));
		}

		public void SetTangentsQuad2(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(1,   -1, 0));
			spline.SetRightTangent(indexTracker, new Vector3(-1, 1,  0));
		}

		public void SetTangentsQuad3(Spline spline, int indexTracker) {
			spline.SetRightTangent(indexTracker, new Vector3(-1, -1, 0));
			spline.SetLeftTangent(indexTracker, new Vector3(1,   1,  0));
		}

		public void SetTangentsQuad34(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(0,  1,  0));
			spline.SetRightTangent(indexTracker, new Vector3(0, -1, 0));
		}

		public void SetTangentsQuad4(Spline spline, int indexTracker) {
			spline.SetLeftTangent(indexTracker, new Vector3(-1, 1,  0));
			spline.SetRightTangent(indexTracker, new Vector3(1, -1, 0));
		}
	}
}