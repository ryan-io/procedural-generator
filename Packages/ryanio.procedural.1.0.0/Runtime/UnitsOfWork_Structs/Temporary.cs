// ProceduralGeneration

using UnityEditor;
using UnityEngine;

namespace ProceduralGeneration.Gizmos {
	internal readonly struct Temporary {
		internal static (float orthoSize, float editorCamSize) GetSceneCameraZoom() {
			SceneView sceneView = SceneView.lastActiveSceneView;
			return (Camera.main.orthographicSize, sceneView.size);
		}
		
		internal static void ZoomSceneCamera(float zoomLevel) {
			SceneView sceneView = SceneView.lastActiveSceneView;
			sceneView.size               = zoomLevel;
			Camera.main.orthographicSize = zoomLevel;
		}
	}
}