using ProceduralAuxiliary;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Engine.Procedural {
	public class PrimitiveCollisionSolver : CollisionSolver {
		protected override Tilemap BoundaryTilemap { get; }
		float SkinWidth { get; }
		float Radius    { get; }


		public override void CreateCollider(CollisionSolverDto dto) {
			var data = dto.MapData;
			
			dto.ColliderGameObject.MakeStatic(true);
			dto.ColliderGameObject.ZeroPosition();

			for (var i = 0; i < data.RoomOutlines.Count; i++) {
				var col              = CreateNewPrimitiveCollider(dto.ColliderGameObject, i.ToString());
				var outline          = data.RoomOutlines[i];
				var extractedCorners = col.corners;

				for (var k = 0; k < 3; k++) {
					var newPoint = new Vector3(
						data.MeshVertices[outline[k]].x, data.MeshVertices[outline[k]].y, 0);
					extractedCorners[k].transform.position = newPoint;
					extractedCorners[k].gameObject.MakeStatic(true);
				}

				for (var j = 3; j < outline.Count; j++) {
					var newPoint = new Vector3(
						data.MeshVertices[outline[j]].x, data.MeshVertices[outline[j]].y, 0);
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
			SkinWidth     = config.PrimitiveColliderRadius;
			Radius        = config.PrimitiveColliderRadius;
			BoundaryTilemap = config.TileMapDictionary[TileMapType.Ground];
		}
	}
}