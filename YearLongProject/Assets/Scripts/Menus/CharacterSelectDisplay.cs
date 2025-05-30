using CharacterScripts;
using Managers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectDisplay : MonoBehaviour
{
    [SerializeField]
    private GameDataSO gameDataSO;

    private Image displayImage;
    private TextMeshProUGUI text;

    [SerializeField]
    private int playerID;

    private void Start()
    {
        displayImage = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        gameDataSO.OnPlayerDataChanged += HandlePlayerDataChanged;
        gameObject.SetActive(false);
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
            return;
        }
        else if (changeType == PlayerDataChange.ProspectCharacterChanged)
        {
            CharacterSO character = gameDataSO.GetPlayerData(playerID).ProspectCharacter;
            displayImage.sprite = null;
            displayImage.color = new Color(0, 0, 0, 0.5f);
            text.text = "";

            if (character == null)
            {
                return;
            }

            displayImage.sprite = character.CharacterPortrait;
            displayImage.color = new Color(1, 1, 1, 0.5f);
            text.text = character.CharacterDisplayName;
        }
        else if (changeType == PlayerDataChange.SelectedCharacterChanged)
        {
            CharacterSO character = gameDataSO.GetPlayerData(playerID).ProspectCharacter;
            displayImage.sprite = null;
            displayImage.color = new Color(1, 1, 1, 0.5f);
            text.text = "";

            if (character == null)
            {
                return;
            }

            displayImage.sprite = character.CharacterPortrait;
            displayImage.color = new Color(1, 1, 1, 1);
            text.text = character.CharacterDisplayName;
        }
    }
}
