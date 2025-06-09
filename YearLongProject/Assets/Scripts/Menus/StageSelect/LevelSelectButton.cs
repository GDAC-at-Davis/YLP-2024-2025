using Input_Scripts;
using LevelScripts;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    /// <summary>
    ///     Button that holds levelSO for level selection
    /// </summary>
    public class LevelSelectButton : ButtonBehavior
    {
        [SerializeField]
        private GameDataSO gameDataSO;

        [SerializeField]
        [Scene]
        private string gameplayScene;

        [SerializeField]
        private Image levelPortrait;

        public LevelSO Level;

        public void Init(LevelSO level)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = level.LevelDisplayName;
            levelPortrait.sprite = level.LevelPortrait;

            button = GetComponent<Button>();
            Level = level;
        }

        public override void OnClick(PlayerCursorController cursor)
        {
            cursor.transform.position = transform.position;

            gameDataSO.SetSelectedLevel(Level);
            gameDataSO.LoadScene(gameplayScene);
        }

        public override void OnHoverEnter(PlayerCursorController cursor)
        {
            gameDataSO.SetProspectLevel(Level);
        }

        public override void OnHoverExit(PlayerCursorController cursor)
        {
            gameDataSO.SetProspectLevel(null);
        }
    }
}