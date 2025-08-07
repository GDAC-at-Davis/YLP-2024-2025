using Animancer;
using Input_Scripts;
using LevelScripts;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.StageSelect
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

        [Header("Unity Events")]

        [SerializeField]
        private UnityEvent onLevelSelected;

        [SerializeField]
        private UnityEvent onHovered;

        [SerializeField]
        private UnityEvent onUnhovered;

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

            onLevelSelected?.Invoke();
        }

        public override void OnHoverEnter(PlayerCursorController cursor)
        {
            gameDataSO.SetProspectLevel(Level);
            onHovered?.Invoke();
        }

        public override void OnHoverExit(PlayerCursorController cursor)
        {
            gameDataSO.SetProspectLevel(null);
            onUnhovered?.Invoke();
        }
    }
}