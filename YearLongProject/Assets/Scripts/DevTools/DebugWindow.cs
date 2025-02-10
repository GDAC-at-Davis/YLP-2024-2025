using UnityEngine;
using UnityEditor;
using Hitbox;

public class DebugWindow : EditorWindow
{

    float timeScale = 1;
    public static bool hitboxSystemDebug = false;

    [MenuItem("Window/DebugMenu")]
    public static void ShowWindow()
    {
        GetWindow<DebugWindow>();
    }

    private void OnGUI()
    {
        timeScale = Mathf.Clamp(EditorGUI.FloatField(new Rect(5, 5, position.width, 20), $"Set timescale to {timeScale}", timeScale), 0, 2);
        hitboxSystemDebug = EditorGUI.Toggle(new Rect(5, 35, position.width, 20), "Toggle Hitbox System Debug", hitboxSystemDebug);
    }

    private void OnInspectorUpdate()
    {
        if (Time.timeScale != timeScale) Time.timeScale = timeScale;  
    }
}
