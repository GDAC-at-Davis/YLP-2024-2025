using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Input_Scripts.Editor
{
    [CustomEditor(typeof(PlayerInputSo))]
    public class PlayerInputSODrawer : UnityEditor.Editor
    {
        private Dictionary<int, PlayerInputSo.PlayerInputEvents> playerInputEvents;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var playerInputSo = (PlayerInputSo)target;

            if (playerInputEvents == null)
            {
                playerInputEvents = playerInputSo.GetType()
                    .GetField("playerInputEvents", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.GetValue(playerInputSo) as Dictionary<int, PlayerInputSo.PlayerInputEvents>;
            }

            if (playerInputEvents == null)
            {
                EditorGUILayout.LabelField("No Player Input Events found.");
                return;
            }

            EditorGUILayout.LabelField($"Player Input Count: {playerInputEvents.Count}");

            foreach (KeyValuePair<int, PlayerInputSo.PlayerInputEvents> kvp in playerInputEvents)
            {
                EditorGUILayout.LabelField($"ID: {kvp.Key}");
            }

            if (GUILayout.Button("Reset"))
            {
                playerInputEvents = new();
                playerInputSo.ClearAllInputReaders();
            }
        }
    }
}