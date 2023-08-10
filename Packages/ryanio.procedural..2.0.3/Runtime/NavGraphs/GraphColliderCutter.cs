using System;
using System.Collections.Generic;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	public class GraphColliderCutter : MonoBehaviour {
		public void Cut(bool forceCache = false) {
			if (_colliders.IsEmptyOrNull() && !forceCache) return;

			_colliders = new HashSet<Collider>(GetComponentsInChildren<Collider>());

			foreach (var col in _colliders)
				switch (_cutType) {
					case CutType.Collision:
						if (col && !col.isTrigger) {
							var gou = new GraphUpdateObject(col.bounds) {
								updatePhysics  = _updateNodesBasedOnPhysics,
								setWalkability = _setWalkability,
								modifyTag      = _setTag
							};

							/*
							// WIP
							// var bounds = gou.bounds;
							// Logger.Test("Bounds : " + bounds);
							//
							// var p1 = bounds.max;
							// var p2 = bounds.min;
							// var p3 = bounds.max - new Vector3(0, -bounds.extents.y, 0);
							// var p4 = bounds.min + new Vector3(0, bounds.extents.y, 0);
							//
							// var points = new Vector3[4] {
							// 	p1, p2, p3, p4
							// };
							//
							// var shape = new GraphUpdateShape(points, false, transform.localToWorldMatrix, 1.0f);

							//gou.shape = shape;
							// var points     = gou.shape.points.ToList();
							// var pointsCopy = new List<Vector3>(points);
							//
							// foreach (var point in points) {
							// 	var cellPosition = _boundaryTilemap.WorldToCell(point);
							// 	var hasTile      = _boundaryTilemap.HasTile(cellPosition);
							// 	if (hasTile)
							// 		pointsCopy.Remove(point);
							// }
							//
							// gou.shape.points = pointsCopy.ToArray();
							*/
							AstarPath.active.UpdateGraphs(gou);
						}

						break;
					case CutType.Trigger:
						if (col && col.isTrigger)
							AstarPath.active.UpdateGraphs(col.bounds);
						break;
					case CutType.Both:
						if (col)
							AstarPath.active.UpdateGraphs(col.bounds);
						break;
				}

			if (_disableOnCut) AstarPath.OnGraphsUpdated += DelayBeforeSetInactive;
		}

		void DelayBeforeSetInactive(AstarPath path) {
			gameObject.SetActive(false);
			AstarPath.OnGraphsUpdated -= DelayBeforeSetInactive;
		}

#if UNITY_EDITOR
		[Button]
		[Title("Editor")]
		void CutEditor() => Cut(true);
#endif

		[Serializable]
		enum CutType {
			Collision,
			Trigger,
			Both
		}
		
		[SerializeField] [EnumToggleButtons] CutType _cutType;

		[SerializeField] bool _updateNodesBasedOnPhysics = true;

		[SerializeField] bool _setWalkability = true;

		[SerializeField] bool _setTag;

		[SerializeField] string _tag;

		[SerializeField] bool _disableOnCut = true;

		HashSet<Collider> _colliders;

		void Awake() {
			_colliders = new HashSet<Collider>(GetComponentsInChildren<Collider>());
		}
	}
}