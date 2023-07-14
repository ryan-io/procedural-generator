using UnityEditor;

namespace MMTools.Editor
{
    [CustomEditor(typeof(MMPlaylist))]
    [CanEditMultipleObjects]

    /// <summary>
    /// A custom editor that displays the current state of a playlist when the game is running
    /// </summary>
    public class MMPlaylistEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MMPlaylist playlist = (MMPlaylist)target;

            DrawDefaultInspector();

            if (playlist.PlaylistState != null)
            {
                EditorGUILayout.LabelField("Current State", playlist.PlaylistState.CurrentState.ToString());
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}