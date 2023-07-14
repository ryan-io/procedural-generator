using UnityEditor;
using UnityEngine;

namespace MMTools.Editor
{
    /// <summary>
    /// Custom editor for the MMTransformRandomizer class
    /// </summary>
    [CustomEditor(typeof(MMTransformRandomizer), true)]
    [CanEditMultipleObjects]
    public class MMTransformRandomizerEditor : UnityEditor.Editor
    {
        /// <summary>
        /// On inspector we handle undo and display a test button
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Undo.RecordObject(target, "Modified MMTransformRandomizer");
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Test", EditorStyles.boldLabel);

            if (GUILayout.Button("Randomize"))
            {
                foreach (MMTransformRandomizer randomizer in targets)
                {
                    randomizer.Randomize();
                }
            }
        }
    }
}
