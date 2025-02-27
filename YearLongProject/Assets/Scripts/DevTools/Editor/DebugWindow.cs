using Animancer.Editor.Tools;
using GBG.PlayableGraphMonitor.Editor;
using Hitbox.System;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevTools.Editor
{
    public class DebugWindow : EditorWindow
    {
        private float timeScale = 1;

        private void OnGUI()
        {
            timeScale = Mathf.Clamp(
                EditorGUILayout.FloatField($"Set timescale to {timeScale}", timeScale), 0, 2);

            HitboxSystemSo.ShowHitboxAreas = EditorGUILayout.Toggle(
                "Toggle Hitbox System Debug", HitboxSystemSo.ShowHitboxAreas);

            if (GUILayout.Button("Reset Scene"))
            {
                if (!Application.isPlaying)
                {
                    return;
                }

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        private void OnInspectorUpdate()
        {
            if (Time.timeScale != timeScale)
            {
                Time.timeScale = timeScale;
            }
        }

        [MenuItem("GDAC YLP/DebugMenu")]
        public static void ShowWindow()
        {
            GetWindow<DebugWindow>();
        }

        [MenuItem("GDAC YLP/Animancer Sprite Tools")]
        public static void ShowAnimancerSpriteToolsWindow()
        {
            GetWindow<AnimancerToolsWindow>();
        }

        [MenuItem("GDAC YLP/Playable Graph Visualizer")]
        public static void ShowPlayableGraphMonitorWindow()
        {
            GetWindow<PlayableGraphMonitorWindow>();
        }

        [MenuItem("GDAC YLP/Timeline/Reset Timeline Shortcut %t")]
        public static void ResetTimelineShortcut()
        {
            Debug.Log("Resetting timeline");
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }
    }
}