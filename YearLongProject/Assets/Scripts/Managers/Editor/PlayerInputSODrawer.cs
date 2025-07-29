using UnityEditor;

namespace Managers.Editor
{
    [CustomEditor(typeof(GameDataSO))]
    public class GameDataSODrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gameData = (GameDataSO)target;

            var i = 0;
            foreach (GameDataSO.PlayerData data in gameData.AllPlayerData)
            {
                EditorGUILayout.LabelField($"{i}: Player ID: {data.PlayerId}");
                i++;
            }
        }
    }
}