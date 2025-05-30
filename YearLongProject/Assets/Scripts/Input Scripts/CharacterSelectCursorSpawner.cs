using EditorUtils.BoldHeader;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Input_Scripts
{
    /// <summary>
    ///     Temporary character select UI component
    ///     Spawns PlayerReadyController when a controller is conencted
    /// </summary>
    public class CharacterSelectCursorSpawner : MonoBehaviour
    {
        public static bool InCharacterSelect;

        [BoldHeader("Player Join UI")]
        [InfoBox("Handles menu stuff when a player joins")]
        [Header("Depends")]

        [SerializeField]
        private GameDataSO gameDataSO;

        [Header("Cursor")]

        [SerializeField]
        private PlayerCursorController cursorPrefab;

        [SerializeField]
        private CharacterSelectDisplay displayPrefab;

        [SerializeField]
        private RectTransform cursorContainer;
        [SerializeField]
        private RectTransform displayContainer;

        [SerializeField]
        private RectTransform cursorBottomLeft;

        [SerializeField]
        private RectTransform cursorTopRight;

        private void OnEnable()
        {
            gameDataSO.OnPlayerDataChanged += OnPlayerDataChanged;
            InCharacterSelect = true;
        }

        private void OnDisable()
        {
            gameDataSO.OnPlayerDataChanged -= OnPlayerDataChanged;
            InCharacterSelect = false;
        }

        private void OnPlayerDataChanged(int priorId, PlayerDataChange changeType, GameDataSO.PlayerData postChangeData)
        {
            if (changeType == PlayerDataChange.PlayerAdded)
            {
                OnPlayerAdded(postChangeData.PlayerId);
            }
        }

        private void OnPlayerAdded(int playerId)
        {
            PlayerCursorController cursor = Instantiate(cursorPrefab, cursorContainer);
            cursor.Initialize(playerId, cursorBottomLeft, cursorTopRight, cursorContainer);
            cursor.BackAction = RemovePlayer;
        }

        void RemovePlayer(PlayerCursorController cursor)
        {
            gameDataSO.RemovePlayer(cursor.PlayerID);
        }
    }
}