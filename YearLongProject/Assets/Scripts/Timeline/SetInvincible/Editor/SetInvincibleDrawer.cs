using EditorUtils.Editor;
using UnityEditor;
using UnityEngine;

namespace Timeline.SetInvincible.Editor
{
    [CustomPropertyDrawer(typeof(SetInvinciblePlayableBehavior))]
    public class SetInvincibleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            YLPEditorGUI.ComponentDescription(position, "Set I-Frames",
                "Gives the Entity I-Frames for the duratin of the clip");
        }
    }
}