using State_Machine_Scripts;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

namespace Timeline.SetTransitionStates
{
    [CustomPropertyDrawer(typeof(SetTransitionStatesPlayableBehavior))]
    public class SetTransitionStatesPlayableBehaviorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SetTransitionStatesPlayableBehavior behavior = ((SetTransitionStatesPlayableAsset)property.serializedObject.targetObject).template;
            string[] states = behavior.actionManager.GetStates();

            behavior.isAllowed = EditorGUILayout.Toggle("Is Allowed?", behavior.isAllowed);
            behavior.flags = EditorGUILayout.MaskField("Set States", behavior.flags, states);

            List<string> allowedStates = new();
            for (var i = 0; i < states.Length; i++)
            {
                // what the fuck am I doing
                var value = (behavior.flags & (1 << i)) != 0;
                if (!value)
                {
                    continue;
                }
                allowedStates.Add(states[i]);
            }
            behavior.allowedStates = allowedStates.ToArray();
        }
    }

    [CustomTimelineEditor(typeof(SetTransitionStatesPlayableAsset))]
    public class SetTransitionStatesPlayableAssetEditor : ClipEditor
    {
        public override void OnClipChanged(TimelineClip clip)
        {
            CharacterActionManager manager = TimelineEditor.inspectedDirector.GetGenericBinding(clip.GetParentTrack()) as CharacterActionManager;
            ((SetTransitionStatesPlayableAsset)clip.asset).template.actionManager = manager;
        }
    }
}