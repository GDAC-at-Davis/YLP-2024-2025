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
    public class PlayerJoinUI : MonoBehaviour
    {
        [BoldHeader("Player Join UI")]
        [InfoBox("Handles menu stuff when a player joins")]
        [Header("Depends")]

        [SerializeField]
        private GameDataSO gameDataSO;

        [SerializeField]
        private PlayerInputSo playerInputSO;

        [Header("Cursor")]

        [SerializeField]
        private PlayerCursorController cursorPrefab;

        [SerializeField]
        private RectTransform container;

        [SerializeField]
        private RectTransform cursorBottomLeft;

        [SerializeField]
        private RectTransform cursorTopRight;

        private void OnEnable()
        {
            playerInputSO.ClearAllInputReaders();
            gameDataSO.ClearPlayerData();
            gameDataSO.OnPlayerDataChanged += OnPlayerDataChanged;
        }

        private void OnDisable()
        {
            gameDataSO.OnPlayerDataChanged -= OnPlayerDataChanged;
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
            PlayerCursorController cursor = Instantiate(cursorPrefab, container);
            cursor.Initialize(playerId, cursorBottomLeft, cursorTopRight);
        }
    }
}