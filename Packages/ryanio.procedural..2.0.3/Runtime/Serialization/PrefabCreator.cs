// ProceduralGeneration
using UnityEditor;
using UnityEngine;

namespace ProceduralGeneration {
	public class PrefabCreator {
		public bool CreateAndSaveAsPrefab(GameObject obj, string rawPath) {
			var uniquePath = AssetDatabase.GenerateUniqueAssetPath(rawPath);
			PrefabUtility.SaveAsPrefabAsset(obj, uniquePath, out var creationSuccess);
			return creationSuccess;
		}

		public PrefabCreator() {
				
		}
	}
}