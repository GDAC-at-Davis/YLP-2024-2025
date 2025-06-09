using System.Collections.Generic;
using CharacterScripts;
using LevelScripts;
using UnityEngine;

namespace Menus.Lore
{
    /// <summary>
    ///     Controls the overall lore page display and behavior.
    /// </summary>
    public class LorePageController : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private LoreSelectButton loreSelectButtonPrefab;

        [SerializeField]
        private CurrentLoreDisplay currentLoreDisplay;

        [Header("Characters")]

        [SerializeField]
        private RectTransform charButtonContainer;

        [SerializeField]
        private CharacterSelectRoster characterSelectRoster;

        [Header("Stages")]

        [SerializeField]
        private RectTransform stageButtonContainer;

        [SerializeField]
        private LevelSelectRoster stageSelectRoster;

        private readonly List<LoreSelectButton> loreButtons = new();

        private void Awake()
        {
            InitializeLoreButtons();
        }

        private void OnDestroy()
        {
            foreach (LoreSelectButton button in loreButtons)
            {
                if (button != null)
                {
                    button.OnLoreSelected.RemoveListener(DisplayLore);
                }
            }
        }

        private void InitializeLoreButtons()
        {
            foreach (CharacterSelectRoster.CharacterSelectData character in characterSelectRoster.Characters)
            {
                CharacterSO charSO = character.Character;

                if (charSO.CharacterLore == null)
                {
                    continue;
                }

                LoreSelectButton button = Instantiate(loreSelectButtonPrefab, charButtonContainer);
                button.Initialize(charSO.CharacterLore, charSO.CharacterDisplayName, charSO.CharacterPortrait);
                button.OnLoreSelected.AddListener(DisplayLore);
                loreButtons.Add(button);
            }

            foreach (LevelSelectRoster.LevelSelectData stage in stageSelectRoster.Levels)
            {
                LevelSO levelSO = stage.Level;

                if (levelSO.LevelLore == null)
                {
                    continue;
                }

                LoreSelectButton button = Instantiate(loreSelectButtonPrefab, stageButtonContainer);
                button.Initialize(levelSO.LevelLore, levelSO.LevelDisplayName, levelSO.LevelPortrait);
                button.OnLoreSelected.AddListener(DisplayLore);
                loreButtons.Add(button);
            }
        }

        private void DisplayLore(LoreSO lore)
        {
            if (lore == null)
            {
                Debug.LogWarning("LoreSO is null. Cannot display lore.");
                return;
            }

            currentLoreDisplay.SetLore(lore);
        }
    }
}