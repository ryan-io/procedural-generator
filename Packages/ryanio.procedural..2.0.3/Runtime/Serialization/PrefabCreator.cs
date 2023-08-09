// ProceduralGeneration

using UnityEditor;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	public class PrefabCreator {
		public bool CreateAndSaveAsPrefab(GameObject obj, string rawPath) {
#if UNITY_EDITOR
			
			var uniquePath = AssetDatabase.GenerateUniqueAssetPath(rawPath);
			PrefabUtility.SaveAsPrefabAsset(obj, rawPath, out var creationSuccess);
			return creationSuccess;
#endif
			return false;
		}

		public PrefabCreator() {
				
		}
	}
}