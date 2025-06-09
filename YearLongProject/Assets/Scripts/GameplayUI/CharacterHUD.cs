using System.Collections.Generic;
using EditorUtils.BoldHeader;
using GameEntities;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayUI
{
    /// <summary>
    ///     Script controlling a single character HUD element
    /// </summary>
    public class CharacterHUD : MonoBehaviour
    {
        [BoldHeader("Character HUD")]
        [InfoBox("A single character's HUD (portrait, healthbar, etc)")]
        [Header("Depends")]

        [SerializeField]
        private GameDataSO gameDataSO;

        [SerializeField]
        private Image portrait;

        [SerializeField]
        private List<Image> tintedImages;

        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private TMP_Text healthText;

        private int playerId;
        private CharacterEntity characterEntity;

        private int maxHealth;

        private void OnDestroy()
        {
            gameDataSO.OnPlayerDataChanged -= OnPlayerDataChanged;
        }

        public void InitializeHUD(int playerId)
        {
            this.playerId = playerId;

            Sprite characterPortrait = gameDataSO.GetPlayerData(playerId).SelectedCharacter.CharacterPortrait;
            Color color = gameDataSO.PlayerColors[playerId];

            portrait.sprite = characterPortrait;

            foreach (Image image in tintedImages)
            {
                image.color = color;
            }

            CharacterEntity newCharEntity = gameDataSO.GetPlayerData(playerId).CharacterEntity;
            SetCharacterEntity(newCharEntity);

            gameDataSO.OnPlayerDataChanged += OnPlayerDataChanged;
        }

        private void OnPlayerDataChanged(int priorId, PlayerDataChange changeType, GameDataSO.PlayerData postChangeData)
        {
            if (postChangeData.PlayerId != playerId)
            {
                return;
            }

            if (changeType == PlayerDataChange.CharacterEntityChanged)
            {
                CharacterEntity newCharacterEntity = postChangeData.CharacterEntity;
                SetCharacterEntity(newCharacterEntity);
            }
        }

        private void SetCharacterEntity(CharacterEntity newCharacterEntity)
        {
            if (newCharacterEntity == null || newCharacterEntity == characterEntity)
            {
                return;
            }

            if (characterEntity != null)
            {
                characterEntity.UpdateHealth -= UpdateHealth;
            }

            characterEntity = newCharacterEntity;
            maxHealth = characterEntity.MaxHealth;
            characterEntity.UpdateHealth += UpdateHealth;
            SetHealthText(maxHealth);
        }

        private void UpdateHealth(int id, int newHealth)
        {
            SetHealthText(newHealth);
        }

        private void SetHealthText(int newHealth)
        {
            if (newHealth < 0)
            {
                newHealth = 0;
            }

            healthBar.value = Mathf.Clamp(newHealth / (float)maxHealth, 0, 1);
            healthText.text = newHealth == 0 ? "" : newHealth.ToString();
        }
    }
}