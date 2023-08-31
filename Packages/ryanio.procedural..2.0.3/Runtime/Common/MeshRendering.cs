// Engine.Procedural

using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralGeneration {
	internal class MeshRendering {
		GameObject           Parent       { get; }
		[CanBeNull] Material InitMaterial { get; }

		internal void Render(MeshData data, string name) {
			name = ValidateName(name);
			var obj = SetupGameObject(name);
			InstantiateMonobehaviors(data.Mesh, obj);
		}

		internal void Render(Mesh mesh, string name) {
			name = ValidateName(name);
			var obj = SetupGameObject(name);
			InstantiateMonobehaviors(mesh, obj);
		}

		GameObject SetupGameObject(string name) {
			var obj = new GameObject(name) {
				transform = {
					parent = Parent.transform
				}
			};
			return obj;
		}

		static string ValidateName(string name) {
			if (string.IsNullOrWhiteSpace(name))
				name = Constants.PROCEDURAL_MESH_NAME;
			return name;
		}

		void InstantiateMonobehaviors(Mesh mesh, GameObject obj) {
			var sorting = obj.AddComponent<SortingGroup>();
			var filter  = obj.AddComponent<MeshFilter>();
			var rend    = obj.AddComponent<MeshRenderer>();

			sorting.sortingOrder = 100;
			filter.mesh          = mesh;

			if (InitMaterial != null)
				rend.material = InitMaterial;
		}

		internal MeshRendering(GameObject parent, [CanBeNull] Material initMaterial) {
			Parent       = parent;
			InitMaterial = initMaterial;
		}
	}
}