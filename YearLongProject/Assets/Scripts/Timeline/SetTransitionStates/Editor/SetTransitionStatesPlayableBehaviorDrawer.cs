using System.Collections.Generic;
using System.Linq;
using EditorUtils.Editor;
using State_Machine_Scripts;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace Timeline.SetTransitionStates.Editor
{
    [CustomPropertyDrawer(typeof(SetTransitionStatesPlayableBehavior))]
    public class SetTransitionStatesPlayableBehaviorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SetTransitionStatesPlayableBehavior behavior =
                ((SetTransitionStatesPlayableAsset)property.serializedObject.targetObject).template;

            YLPEditorGUI.ComponentDescription(position, "Set Allowed States",
                "Set which states the character can transition to during this clip." +
                " All other states cannot be transitioned to.");

            if (behavior.ActionManager == null)
            {
                EditorGUILayout.HelpBox(
                    "Inspect through a PlayableDirector on a gameobject to list all states.",
                    MessageType.Warning);
            }

            // If we're inspecting without a director, we can't get the action manager, so just assume the existing listed states
            StateNameSO[] currentStates = behavior.ActionManager == null
                ? behavior.AllowedStates
                : behavior.ActionManager.GetStates();
            StateNameSO[] behaviorStates = behavior.AllowedStates;

            // Mask field can't draw empty lists, so just exit early
            if (currentStates.Length == 0)
            {
                return;
            }

            // Convert into integer mask
            var flags = 0;
            for (var i = 0; i < currentStates.Length; i++)
            {
                // what the fuck am I doing
                // So real king
                StateNameSO state = currentStates[i];

                // The existing mask has this state, so we add it to the integer mask
                if (behaviorStates.Contains(state))
                {
                    flags |= 1 << i;
                }
            }

            flags = EditorGUILayout.MaskField("Allowed State Transitions",
                flags,
                currentStates.Select(a => (string)a).ToArray());

            // Convert int mask back into list of StateNames
            List<StateNameSO> allowedStates = new();
            for (var i = 0; i < currentStates.Length; i++)
            {
                // what the fuck am I doing
                bool value = (flags & (1 << i)) != 0;
                if (!value)
                {
                    continue;
                }

                allowedStates.Add(currentStates[i]);
            }

            // Compare old and new to set dirty and record undo if needed
            var shouldSetDirty = false;
            if (behavior.AllowedStates.Length == allowedStates.Count)
            {
                for (var i = 0; i < behavior.AllowedStates.Length; i++)
                {
                    if (behaviorStates[i] != behavior.AllowedStates[i])
                    {
                        shouldSetDirty = true;
                    }
                }
            }
            else
            {
                shouldSetDirty = true;
            }

            if (shouldSetDirty)
            {
                Undo.RecordObject(TimelineEditor.selectedClip.asset, "Edited Allowed State");
                EditorUtility.SetDirty(TimelineEditor.inspectedAsset);
            }

            behavior.AllowedStates = allowedStates.ToArray();
        }
    }

    [CustomTimelineEditor(typeof(SetTransitionStatesPlayableAsset))]
    public class SetTransitionStatesPlayableAssetEditor : ClipEditor
    {
        public override void OnClipChanged(TimelineClip clip)
        {
            if (TimelineEditor.inspectedDirector == null)
            {
                return;
            }

            // Only way to get a reference to the action manager for the editor drawer
            var manager =
                TimelineEditor.inspectedDirector.GetGenericBinding(clip.GetParentTrack()) as CharacterActionManager;
            ((SetTransitionStatesPlayableAsset)clip.asset).template.ActionManager = manager;
        }
    }
}