using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Engine.Procedural.Runtime {
	public abstract class CollisionSolver {
		protected abstract Tilemap       BoundaryTilemap      { get; }
		
		public abstract void CreateCollider(CollisionSolverDto dto, List<Vector3> cache, 
			[CallerMemberName] string caller = "");

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
	}
}