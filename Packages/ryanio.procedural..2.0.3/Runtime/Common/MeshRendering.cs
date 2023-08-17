// Engine.Procedural

using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralGeneration {
	internal class MeshRendering {
		GameObject           Parent       { get; }
		[CanBeNull] Material InitMaterial { get; }

		internal void Render(MeshData data, string name) {
			if (string.IsNullOrWhiteSpace(name))
				name = Constants.PROCEDURAL_MESH_NAME;

			var obj = new GameObject(name) {
				transform = {
					parent = Parent.transform
				}
			};

			var sorting = obj.AddComponent<SortingGroup>();
			var filter  = obj.AddComponent<MeshFilter>();
			var rend    = obj.AddComponent<MeshRenderer>();

			sorting.sortingOrder = 100;
			filter.mesh          = data.Mesh;

			if (InitMaterial != null)
				rend.material = InitMaterial;
		}

		internal MeshRendering(GameObject parent, [CanBeNull] Material initMaterial) {
			Parent       = parent;
			InitMaterial = initMaterial;
		}
	}
}