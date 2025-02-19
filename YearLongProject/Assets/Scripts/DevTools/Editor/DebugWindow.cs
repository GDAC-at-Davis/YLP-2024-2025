using Hitbox.System;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugWindow : EditorWindow
{
    private float timeScale = 1;

    private void OnGUI()
    {
        timeScale = Mathf.Clamp(
            EditorGUI.FloatField(new Rect(5, 5, position.width, 20), $"Set timescale to {timeScale}", timeScale), 0, 2);
        HitboxSystemSo.ShowHitboxAreas = EditorGUI.Toggle(new Rect(5, 30, position.width, 20),
            "Toggle Hitbox System Debug", HitboxSystemSo.ShowHitboxAreas);
        if (GUI.Button(new Rect(5, 55, position.width, 20), "Reset Scene"))
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

    [MenuItem("Debug/DebugMenu")]
    public static void ShowWindow()
    {
        GetWindow<DebugWindow>();
    }

    [MenuItem("Debug/Reset Timeline Shortcut %t")]
    public static void ResetTimelineShortcut()
    {
        Debug.Log("Resetting timeline");
        TimelineEditor.Refresh(RefreshReason.ContentsModified);
    }
}