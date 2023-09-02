using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProceduralGeneration {
	internal abstract class CollisionSolver {
		internal abstract Coordinates CreateCollider([CallerMemberName] string caller = "");

		protected GameObject AddRoom(GameObject parent, string identifier = "", params Type[] componentsToAdd) {
			var newObj = new GameObject($"room {identifier} - colliders") {
				transform = {
					parent = parent.transform
				}
			};
			
			if (!componentsToAdd.IsEmptyOrNull()) {
				
			}
			foreach (var component in componentsToAdd)
				newObj.AddComponent(component);

			return newObj;
		}
	}
}