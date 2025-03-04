using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class YLPEditorGUI
    {
        public static void ComponentHeader(Rect position, string name, string help)
        {
            GUI.Label(position, name, EditorStyles.boldLabel);
            position.y += EditorGUIUtility.singleLineHeight * 1.25f;
            NaughtyEditorGUI.HorizontalLine(position, 2, Color.white);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                help,
                MessageType.Info);
        }
    }
}