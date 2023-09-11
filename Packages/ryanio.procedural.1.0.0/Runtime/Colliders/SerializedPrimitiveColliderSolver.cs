// ProceduralGeneration

using System.Collections.Generic;
using BCL;
using ProceduralAuxiliary;
using ProceduralAuxiliary.ProceduralCollider;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	internal class SerializedPrimitiveCollisionSolver {
		GameObject ColliderGo { get; }
		float      SkinWidth  { get; }
		float      Radius     { get; }

		internal void CreateColliderFromDict(Dictionary<int, List<Vector3>> dict) {
			ColliderGo.MakeStatic(true);
			ColliderGo.ZeroPosition();

			int tracker = 0;

			if (dict.IsEmptyOrNull()) {
				return;
			}
			
			foreach (var pair in dict) {
				InstantiateCollider(pair.Value, tracker);
				tracker++;
			}
		}

		void InstantiateCollider(IReadOnlyList<Vector3> outlines, int index) {
			if (outlines.Count < 3)
				return;
			
			var col     = CreateNewPrimitiveCollider(index.ToString());
			var objList = new GameObject[3];
			
			for (var k = 0; k < 3; k++) {
				CreateOriginColliders(outlines[k], k, col, objList);
			}

			for (var j = 0; j < outlines.Count; j++) {
				CreateBodyColliders(outlines[j], col);
			}

			foreach (var obj in objList) {
				if (Application.isEditor)
					Object.DestroyImmediate(obj);
				else
					Object.Destroy(obj);
			}
		}

		void CreateOriginColliders(Vector3 point, int k, ProceduralPrimitiveCollider col, IList<GameObject> objList) {
			col.corners[k].transform.position = point;
			col.corners[k].gameObject.MakeStatic(true);
			objList[k] = col.corners[k].gameObject;
		}

		void CreateBodyColliders(Vector3 point, ProceduralPrimitiveCollider col) {
			//CreateHandle(col, point, col.corners[^1], col.corners[^1].GetSiblingIndex() + 1);
			var index = col.corners[^1].GetSiblingIndex() + 1;
			var newCorner = Object.Instantiate(
				col.corners[^1], point, Quaternion.identity, col.transform);

			newCorner.gameObject.MakeStatic(true);
			newCorner.SetSiblingIndex(index);
		}

		ProceduralPrimitiveCollider CreateNewPrimitiveCollider(string identifier) {
			var obj = new GameObject {
				name = $"room {identifier} - colliders",
				transform = {
					position = Vector3.zero,
					parent   = ColliderGo.transform
				}
			};

			obj.MakeStatic(true);
			obj.transform.eulerAngles = new Vector3(90, 0, 0);

			var col = obj.AddComponent<ProceduralPrimitiveCollider>();
			InjectSettings(col);

			for (var i = 0; i < 3; i++) {
				var newObj = new GameObject().transform;
				newObj.SetParent(col.gameObject.transform);
				newObj.localPosition   = Vector3.forward * (0.5f * 5 * i);
				newObj.gameObject.name = i.ToString();
				col.corners.Add(newObj);
			}

			return col;
		}

		void InjectSettings(ProceduralPrimitiveCollider col) {
			col.depth            = SkinWidth / 2f;
			col.heigth           = SkinWidth / 2f;
			col.radius           = Radius;
			col.onlyWhenSelected = true;
			col.loop             = true;
		}

		internal SerializedPrimitiveCollisionSolver(SerializedPrimitiveCollisionSolverCtx ctx) {
			ColliderGo = ctx.ColliderGo;
			SkinWidth  = ctx.SkinWidth;
			Radius     = ctx.Radius;
		}
	}
}