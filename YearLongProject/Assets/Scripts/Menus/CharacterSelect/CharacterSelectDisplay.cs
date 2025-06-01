using CharacterScripts;
using Managers;
using Menus.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Menus.CharacterSelect
{
    public class CharacterSelectDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameDataSO gameDataSO;

        [SerializeField]
        private TMP_Text characterNameText;

        [SerializeField]
        private TextScrollRegion characterLoreTextRegion;

        [SerializeField]
        private Image characterImage;

        [SerializeField]
        private int playerID;

        private void Start()
        {
            gameDataSO.OnPlayerDataChanged += HandlePlayerDataChanged;
            gameObject.SetActive(false);

            characterLoreTextRegion.SetVisible(false);
        }

        private void OnDestroy()
        {
            gameDataSO.OnPlayerDataChanged -= HandlePlayerDataChanged;
        }

        private void HandlePlayerDataChanged(int priorid, PlayerDataChange changeType,
            GameDataSO.PlayerData postchangedata)
        {
            if (priorid != playerID)
            {
                return;
            }

            if (changeType == PlayerDataChange.PlayerAdded)
            {
                gameObject.SetActive(true);
                return;
            }

            if (changeType == PlayerDataChange.PlayerRemoved)
            {
                gameObject.SetActive(false);
                characterLoreTextRegion.SetVisible(false);

                return;
            }

            if (changeType == PlayerDataChange.ProspectCharacterChanged)
            {
                CharacterSO character = gameDataSO.GetPlayerData(playerID).ProspectCharacter;
                characterImage.sprite = null;
                characterImage.color = new Color(0, 0, 0, 0.5f);
                characterNameText.text = "";

                if (character == null)
                {
                    characterLoreTextRegion.SetVisible(false);
                    return;
                }

                characterImage.sprite = character.CharacterPortrait;
                characterImage.color = new Color(1, 1, 1, 0.5f);
                characterNameText.text = character.CharacterDisplayName;

                characterLoreTextRegion.SetText(character.CharacterLoreDescription);
                characterLoreTextRegion.SetVisible(true);
            }
            else if (changeType == PlayerDataChange.SelectedCharacterChanged)
            {
                CharacterSO character = gameDataSO.GetPlayerData(playerID).ProspectCharacter;
                characterImage.sprite = null;
                characterImage.color = new Color(1, 1, 1, 0.5f);
                characterNameText.text = "";

                if (character == null)
                {
                    return;
                }

                characterImage.sprite = character.CharacterPortrait;
                characterImage.color = new Color(1, 1, 1, 1);
                characterNameText.text = character.CharacterDisplayName;
            }
        }
    }
}