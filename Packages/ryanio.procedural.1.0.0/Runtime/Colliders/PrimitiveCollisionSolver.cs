using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProceduralAuxiliary.ProceduralCollider;
using UnityBCL;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	/// <summary>
	/// Class for solving primitive collisions.
	/// </summary>
	internal class PrimitiveCollisionSolver : CollisionSolver {
		/// <summary>
		/// Gets the GameObject associated with this Collider.
		/// </summary>
		/// <returns>The GameObject associated with this Collider.</returns>
		/// <remarks>
		/// The GameObject represents the object that contains the Collider component.
		/// </remarks>
		GameObject ColliderGo { get; }

		/// <summary>
		/// Gets the outlines of rooms in a 2D space.
		/// </summary>
		/// <remarks>
		/// Each room is represented by a list of integers, where each integer represents a point in the room's outline.
		/// The list of rooms is a list of these room outline lists.
		/// </remarks>
		/// <value>
		/// The outlines of rooms in a 2D space.
		/// </value>
		List<List<int>> RoomOutlines { get; }

		/// <summary>
		/// Gets or sets the index of the processed coordinate.
		/// </summary>
		/// <value>
		/// The index of the processed coordinate.
		/// </value>
		int ProcessedCoordIndex { get; set; }

		/// <summary>
		/// Gets the skin width of an object.
		/// </summary>
		/// <remarks>
		/// The skin width is used to allow for small overlaps between colliders before they are considered to be contacting each other.
		/// </remarks>
		/// <value>
		/// The skin width of the object.
		/// </value>
		float SkinWidth { get; }

		/// <summary>
		/// Creates a collider for the room using the given outlines. </summary> <param name="caller">The name of the calling member.</param> <returns>The coordinates of the processed points for the collider.</returns>
		/// /
		internal override Coordinates CreateCollider([CallerMemberName] string caller = "") {
			ColliderGo.MakeStatic(true);
			ColliderGo.ZeroPosition();

			for (var outlineIndex = 0; outlineIndex < RoomOutlines.Count; outlineIndex++) {
				var currentOutline = RoomOutlines[outlineIndex];
				var points         = Processor.Process(outlineIndex, ref currentOutline);

				ProcessCoords(ref points, outlineIndex);
			}

			CoreExtensions.SetLayerRecursive(ColliderGo, LayerMask.NameToLayer(Constants.Layer.BOUNDARY));

			return new Coordinates(Processor.ProcessedPoints);
		}

		/// <summary>
		/// Processes coordinates to create primitive colliders and handles.
		/// </summary>
		/// <param name="processedPoints">The list of processed points.</param>
		/// <param name="currentOutlineIndex">The index of the current outline.</param>
		/// <remarks>
		/// This method takes in a list of processed points and the index of the current outline.
		/// It checks if the count of the current outline is less than or equal to 10, and if so, returns immediately.
		/// If the count is greater than 10, it creates a new primitive collider based on the current outline index.
		/// It then sets the starting references using the processed points and the primitive collider.
		/// Next, it iterates through each point in the processed points list and creates a handle using the
		/// primitive collider, the current point, the last corner of the collider, and the processed coordinate index.
		/// Finally, it destroys the temporary objects in the list based on the current execution environment (editor or runtime).
		/// </remarks>
		void ProcessCoords(ref List<Vector2> processedPoints, int currentOutlineIndex) {
			var currentOutline = RoomOutlines[currentOutlineIndex];

			if (currentOutline.Count <= 10)
				return;

			var primitiveCollider = CreateNewPrimitiveCollider(currentOutlineIndex.ToString());
			var tempObjectList    = SetStarting(ref processedPoints, ref primitiveCollider);

			foreach (var point in processedPoints) {
				CreateHandle(primitiveCollider, point, primitiveCollider.corners[^1], ProcessedCoordIndex);
				ProcessedCoordIndex++;
			}

			foreach (var obj in tempObjectList) {
				if (Application.isEditor)
					Object.DestroyImmediate(obj);
				else
					Object.DestroyImmediate(obj);
			}
		}

		/// <summary>
		/// Sets up the starting colliders for the provided processed points and procedural primitive collider.
		/// </summary>
		/// <param name="processedPoints">The processed points used to set up the starting colliders.</param>
		/// <param name="col">The procedural primitive collider used to set up the starting colliders.</param>
		/// <returns>An enumerable collection of GameObjects representing the starting colliders.</returns>
		IEnumerable<GameObject> SetStarting(ref List<Vector2> processedPoints, ref ProceduralPrimitiveCollider col) {
			var tempObjectList = new GameObject[3];

			for (var index = 0; index < 3; index++)
				tempObjectList[index] = SetupStartingCollider(processedPoints[index], index, col);

			return tempObjectList;
		}

		/// <summary>
		/// Sets up the starting collider by positioning it at the specified point, making it static, and returns the game object of the collider corner.
		/// </summary>
		/// <param name="point">The position where the collider corner will be positioned.</param>
		/// <param name="k">The index of the collider corner in the ProceduralPrimitiveCollider's corners collection.</param>
		/// <param name="col">The ProceduralPrimitiveCollider instance containing the corners collection.</param>
		/// <returns>The game object of the collider corner.</returns>
		static GameObject SetupStartingCollider(Vector3 point, int k, ProceduralPrimitiveCollider col) {
			col.corners[k].transform.position = point;
			col.corners[k].gameObject.MakeStatic(true);
			return col.corners[k].gameObject;
		}

		/// <summary>
		/// Creates a new ProceduralPrimitiveCollider with the specified identifier.
		/// </summary>
		/// <param name="identifier">The identifier to be used for naming the collider.</param>
		/// <returns>A new instance of ProceduralPrimitiveCollider.</returns>
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

		/// <summary>
		/// Injects settings into the provided ProceduralPrimitiveCollider object.
		/// </summary>
		/// <param name="col">
		/// The ProceduralPrimitiveCollider object to inject the settings into.
		/// </param>
		void InjectSettings(ProceduralPrimitiveCollider col) {
			col.loop             = true;
			col.depth            = SkinWidth / 2f;
			col.heigth           = SkinWidth / 2f;
			col.radius           = SkinWidth;
			col.onlyWhenSelected = true;
		}

		/// <summary>
		/// Creates a handle by instantiating a corner prototype and placing it at the specified position.
		/// </summary>
		/// <param name="easyWallCollider">The parent component to which the handle will be added.</param>
		/// <param name="newPos">The position at which the handle will be placed.</param>
		/// <param name="cornerPrototype">The prototype of the corner object to be instantiated.</param>
		/// <param name="newIndex">The index at which the handle will be placed in the sibling hierarchy.</param>
		/// <remarks>
		/// This method creates a handle by instantiating a corner prototype object and adding it as a child to the given
		/// <paramref name="easyWallCollider"/> component. The handle is placed at the specified <paramref name="newPos"/>
		/// and positioned at the <paramref name="newIndex"/> within the sibling hierarchy of the parent.
		/// Before adding the handle, its game object is made static to improve performance.
		/// </remarks>
		/// <seealso cref="UnityEngine.Object.Instantiate(UnityEngine.Object,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Transform)"/>
		/// <seealso cref="UnityEngine.Transform.SetSiblingIndex(int)"/>
		static void CreateHandle(Component easyWallCollider, Vector3 newPos, Transform cornerPrototype, int newIndex) {
			var newCorner = Object.Instantiate(
				cornerPrototype, newPos, Quaternion.identity, easyWallCollider.transform);

			newCorner.gameObject.MakeStatic(true);
			newCorner.SetSiblingIndex(newIndex);
		}

		/// <summary>
		/// Represents a PrimitiveCollisionSolver object.
		/// </summary>
		/// <param name="ctx">The ColliderSolverCtx object used to initialize the PrimitiveCollisionSolver.</param>
		/// <param name="meshVertices">A reference to the list of Vector3 containing mesh vertices.</param>
		/// <param name="cutOffPoints">The cutoff points for the PrimitiveCollisionSolver. Default value is 10.</param>
		public PrimitiveCollisionSolver(ColliderSolverCtx ctx, ref List<Vector3> meshVertices,
			int cutOffPoints = 10)
			: base(ref meshVertices, cutOffPoints) {
			SkinWidth    = ctx.SkinWidth;
			ColliderGo   = ctx.ColliderGo;
			RoomOutlines = ctx.RoomOutlines;
		}
	}
}