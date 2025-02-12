using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Hitbox;

public class DebugWindow : EditorWindow
{

    float timeScale = 1;

    [MenuItem("Debug/DebugMenu")]
    public static void ShowWindow()
    {
        GetWindow<DebugWindow>();
    }

    private void OnGUI()
    {
        timeScale = Mathf.Clamp(EditorGUI.FloatField(new Rect(5, 5, position.width, 20), $"Set timescale to {timeScale}", timeScale), 0, 2);
        HitboxSystemSo.ShowHitboxAreas = EditorGUI.Toggle(new Rect(5, 30, position.width, 20), "Toggle Hitbox System Debug", HitboxSystemSo.ShowHitboxAreas);
        if (GUI.Button(new Rect(5, 55, position.width, 20), "Reset Scene"))
        {
            if (!Application.isPlaying) return;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnInspectorUpdate()
    {
        if (Time.timeScale != timeScale) Time.timeScale = timeScale;  
    }
}
