using UnityEditor;
using UnityEngine;

namespace MMTools.Editor
{
    /// <summary>
    /// Custom editor for the MMTilemapGenerator, handles generate button and reorderable layers
    /// </summary>
    [CustomEditor(typeof(MMTilemapGenerator), true)]
    [CanEditMultipleObjects]
    public class MMTilemapGeneratorEditor : UnityEditor.Editor
    {
    
        protected MMReorderableList _list;

        protected virtual void OnEnable()
        {
            _list = new MMReorderableList(serializedObject.FindProperty("Layers"));
            _list.elementNameProperty = "Layer";
            _list.elementDisplayType = MMReorderableList.ElementDisplayType.Expandable;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawPropertiesExcluding(serializedObject,  "Layers");
            EditorGUILayout.Space(10);
            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            
            if (GUILayout.Button("Generate"))
            {
                (target as MMTilemapGenerator).Generate();
            }
        }
    }
}
