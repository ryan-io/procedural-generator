using System;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.ColliderSolver {
	public abstract class CollisionSolver {
		public abstract void CreateCollider(CollisionSolverDto dto);

		protected GameObject AddRoom(GameObject parent, string identifier = "", params Type[] componentsToAdd) {
			var newObj = new GameObject($"Room {identifier} - Colliders") {
				transform = {
					parent = parent.transform
				}
			};

			if (!componentsToAdd.IsEmptyOrNull())
				foreach (var component in componentsToAdd)
					newObj.AddComponent(component);

			return newObj;
		}
	}
}