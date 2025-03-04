using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEngine;

namespace EditorUtils.Editor
{
    public class YLPEditorGUI
    {
        public static Color headerColor = new(1f, 0.973f, 0.639f);

        public static GUIStyle BoldHeaderStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.largeLabel);
                style.normal.textColor = headerColor;
                return style;
            }
        }

        /// <summary>
        ///     Combination of a bold header and a help box.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="name"></param>
        /// <param name="help"></param>
        public static void ComponentDescription(Rect position, string name, string help)
        {
            DrawBoldHeader(position, name);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                help,
                MessageType.Info);
        }

        /// <summary>
        ///     Draws a text header with a horizontal line underneath it.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        public static void DrawBoldHeader(Rect position, string text)
        {
            position.y += EditorGUIUtility.singleLineHeight * 0.6f;
            GUI.Label(position, text, BoldHeaderStyle);
            position.y += EditorGUIUtility.singleLineHeight * 1.2f;
            NaughtyEditorGUI.HorizontalLine(position, 2, headerColor);
        }
    }
}