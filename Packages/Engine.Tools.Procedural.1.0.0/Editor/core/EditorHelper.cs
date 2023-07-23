using System.Reflection;

namespace Engine.Procedural.Editor.Editor {
	public static class EditorHelper {
		public static void ClearConsole() {
#if UNITY_EDITOR || UNITY_STANDALONE
			var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
			var type     = assembly.GetType("UnityEditor.LogEntries");
			var method   = type.GetMethod("Clear");
			method?.Invoke(new object(), null);
#endif
		}
	}
}