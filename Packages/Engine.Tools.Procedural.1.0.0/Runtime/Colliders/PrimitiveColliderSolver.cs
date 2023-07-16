using ProceduralAuxiliary;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.ColliderSolver {
	public class PrimitiveCollisionSolver : CollisionSolver {
		float SkinWidth { get; }
		float Radius    { get; }

		public override void CreateCollider(CollisionSolverDto dto) {
			dto.ColliderGameObject.MakeStatic(true);
			dto.ColliderGameObject.ZeroPosition();

			for (var i = 0; i < dto.Outlines.Count; i++) {
				var col              = CreateNewPrimitiveCollider(dto.ColliderGameObject, i.ToString());
				var outline          = dto.Outlines[i];
				var extractedCorners = col.corners;

				for (var k = 0; k < 3; k++) {
					var newPoint = new Vector3(
						dto.WalkableVertices[outline[k]].x, dto.WalkableVertices[outline[k]].y, 0);
					extractedCorners[k].transform.position = newPoint;
					extractedCorners[k].gameObject.MakeStatic(true);
				}

				for (var j = 3; j < outline.Count; j++) {
					var newPoint = new Vector3(
						dto.WalkableVertices[outline[j]].x, dto.WalkableVertices[outline[j]].y, 0);
					CreateHandle(col, newPoint, col.corners[^1], col.corners[^1].GetSiblingIndex() + 1);
				}
			}

			CoreExtensions.SetLayerRecursive(dto.ColliderGameObject, LayerMask.NameToLayer("Boundary"));
		}

		ProceduralPrimitiveCollider CreateNewPrimitiveCollider(GameObject parent, string identifier) {
			var obj = new GameObject {
				name = $"Primitive Collider - Room {identifier}",
				transform = {
					position = Vector3.zero,
					parent   = parent.transform
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
#if UNITY_EDITOR
				//PolygonColliderEditorExtention.DrawIcon(newObj.gameObject, 0);
#endif
				col.corners.Add(newObj);
			}

			//orcePopulateCorners();
			return col;
		}

		void InjectSettings(ProceduralPrimitiveCollider col) {
			col.depth            = SkinWidth / 2f;
			col.heigth           = SkinWidth / 2f;
			col.radius           = Radius;
			col.onlyWhenSelected = true;
		}

		void CreateHandle(
			Component easyWallCollider, Vector3 newPos, Transform cornerPrototype, int newIndex) {
			var newCorner = Object.Instantiate(
				cornerPrototype, newPos, Quaternion.identity, easyWallCollider.transform);

			newCorner.gameObject.MakeStatic(true);
			newCorner.SetSiblingIndex(newIndex);
		}

		public PrimitiveCollisionSolver(ProceduralConfig config) {
			SkinWidth = config.PrimitiveColliderRadius;
			Radius    = config.PrimitiveColliderRadius;
		}
	}
}