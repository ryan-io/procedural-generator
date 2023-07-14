using System;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Engine.Procedural {
	[Serializable]
	public class MeshSerialization {
		[Serializable]
		public enum SerializationType {
			Combined,
			Individual,
			Both
		}

		const string FolderName = "Procedural Meshes";

		[SerializeField] [Title("Required Monobehaviors")] [Required]
		ProceduralMeshSolver _meshSolver;

		bool ShouldFix => !_meshSolver;

		[Button]
		[Title("Mesh Injection")]
		public void Inject(SerializationType type, string filePrefix, bool simplify = true,
			float simplification = 0.7f) {
			var generatedData = _meshSolver.GetGeneratedData;

			if (!_meshSolver || !generatedData.IsCompleted || string.IsNullOrWhiteSpace(filePrefix)) {
				var log = new UnityLogging();
				log.Error("Mesh was null or a valid file name was not provided.");
				return;
			}

			switch (type) {
				case SerializationType.Combined:
					ProcessCombined(generatedData, filePrefix, simplify, simplification);
					break;
				case SerializationType.Individual:
					ProcessIndividual(generatedData, filePrefix, simplify, simplification);
					break;
				case SerializationType.Both:
					ProcessCombined(generatedData, filePrefix, simplify, simplification);
					ProcessIndividual(generatedData, filePrefix, simplify, simplification);
					break;
			}
		}

		void ProcessCombined(MeshGenerationData generatedData, string filePrefix, bool simplify,
			float simplification) {
			var generatedMesh = generatedData.GeneratedMesh;
			var mesh = simplify
				           ? MeshSimplification.Simply(generatedMesh, simplification)
				           : generatedMesh;

			var name    = filePrefix + "_" + generatedData.MeshSeed + "_" + DateTime.Now.ToString("hh-mm-ss-MM-yyyy");
			// var generic = new GenericSaver(FolderName);
			// generic.Save(mesh, name, true);
			var log = new UnityLogging();
			log.Msg($"Saved {mesh.name} as {name}");
		}

		void ProcessIndividual(MeshGenerationData generatedData, string filePrefix, bool simplify,
			float simplification) {
			var collection = generatedData.GeneratedRoomMeshes;

			foreach (var mesh in collection) {
				var serializedMesh = simplify
					                     ? MeshSimplification.Simply(mesh.Value, simplification)
					                     : mesh.Value;

				var name = filePrefix + "-" + serializedMesh.name + "_" + generatedData.MeshSeed
				           + "_"      + DateTime.Now.ToString("hh-mm-ss-MM-yyyy");

				// var generic = new GenericSaver(FolderName + $" - {generatedData.MeshSeed} Room Meshes");
				// generic.Save(serializedMesh, name, true);
				var log = new UnityLogging();
				log.Msg($"Saved {serializedMesh.name} as {name}");
			}
		}

		[Button]
		[ShowIf("@ShouldFix")]
		void Fix() {
			_meshSolver = Object.FindObjectOfType<ProceduralMeshSolver>();
		}
	}
}