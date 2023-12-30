using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	/// Represents an abstract collision solver for managing collisions between game objects.
	/// </summary>
	internal abstract class CollisionSolver: IDisposable {
		/// <summary>
		/// Gets or sets a value indicating whether the object has been disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
		/// </value>
		protected bool IsDisposed { get; set; }

		/// <summary>
		/// The Processor property represents the protected BoundaryPointProcessor instance. </summary>
		/// <value>
		/// The Processor property is a read-only property of type BoundaryPointProcessor. </value>
		/// /
		protected BoundaryPointProcessor Processor { get; }

		/// <summary>
		/// Creates a collider using the specified caller name as an optional parameter. </summary> <param name="caller">
		/// The name of the calling method. This parameter is optional and will be automatically filled with the name of the method calling this method. </param> <returns>
		/// An instance of the <see cref="Coordinates"/> class representing the newly created collider. </returns>
		/// /
		internal abstract Coordinates CreateCollider([CallerMemberName] string caller = "");

		/// <summary>
		/// Adds a new room to the parent GameObject and optionally adds specified components to the room.
		/// </summary>
		/// <param name="parent">The parent GameObject to add the room under.</param>
		/// <param name="identifier">Optional identifier for the room.</param>
		/// <param name="componentsToAdd">Optional list of components to add to the room.</param>
		/// <returns>The newly created room GameObject.</returns>
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

		/// <summary>
		/// Releases the managed resources used by this object.
		/// </summary>
		public virtual void Dispose() {
			// TODO release managed resources here
		}

		/// <summary>
		/// Represents a CollisionSolver object that is used to solve collisions between objects.
		/// </summary>
		/// <param name="meshVertices">A reference to a list of Vector3 objects representing the mesh vertices.</param>
		/// <param name="cutOffPoints">The number of cut off points for the BoundaryPointProcessor. The default value is 10.</param>
		protected CollisionSolver(ref List<Vector3> meshVertices, int cutOffPoints = 10) {
			Processor = new BoundaryPointProcessor(ref meshVertices, cutOffPoints);
		}
		
		
	}
}