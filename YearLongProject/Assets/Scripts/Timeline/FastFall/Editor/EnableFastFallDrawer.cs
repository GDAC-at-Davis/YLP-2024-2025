using EditorUtils.Editor;
using Timeline.LockFlipX;
using UnityEditor;
using UnityEngine;

namespace Timeline.FastFall.Editor
{
    [CustomPropertyDrawer(typeof(LockFlipXPlayableBehavior))]
    public class EnableFastFallDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            YLPEditorGUI.ComponentDescription(position, "Lock Flip X",
                "Prevents the character from changing horizontal directions for the duration of the clip.");
        }
    }
}