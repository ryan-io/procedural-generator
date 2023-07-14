using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MMTools.Editor
{
    [InitializeOnLoad]
    public static class MMMonoBehaviourDrawerHelper
    {
        public static void DrawButton(this UnityEditor.Editor editor, MethodInfo methodInfo)
        {
            if (GUILayout.Button(methodInfo.Name))
            {
                methodInfo.Invoke(editor.target, null);
            }
        }

        public static void DrawVerticalLayout(this UnityEditor.Editor editor, Action action, GUIStyle style)
        {
            EditorGUILayout.BeginVertical(style);
            action();
            EditorGUILayout.EndVertical();
        }
    }
}