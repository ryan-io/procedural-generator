using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Engine.Procedural.Runtime {
	public abstract class CollisionSolver {
		public             Dictionary<int, List<Vector3>> AllBoundaryPoints { get; }
		protected abstract Tilemap                        BoundaryTilemap   { get; }

		public abstract Dictionary<int, List<Vector3>> CreateCollider(
			CollisionSolverDto dto, [CallerMemberName] string caller = "");

		protected GameObject AddRoom(GameObject parent, string identifier = "", params Type[] componentsToAdd) {
			var newObj = new GameObject($"room {identifier} - colliders") {
				transform = {
					parent = parent.transform
				}
			};

			if (!componentsToAdd.IsEmptyOrNull())
				foreach (var component in componentsToAdd)
					newObj.AddComponent(component);

			return newObj;
		}

		public CollisionSolver() {
			AllBoundaryPoints = new Dictionary<int, List<Vector3>>();
		}
	}
}