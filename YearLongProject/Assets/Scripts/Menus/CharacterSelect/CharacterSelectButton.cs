using Animancer;
using CharacterScripts;
using Input_Scripts;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Managers.GameDataSO;

namespace Menus
{
    /// <summary>
    ///     Button that holds characterSO for character selection
    /// </summary>
    public class CharacterSelectButton : ButtonBehavior
    {
        [SerializeField]
        private GameDataSO gameDataSO;

        public CharacterSO Character;
        public GridLayoutGroup LayoutGroup;

        [SerializeField]
        private Image[] markers;

        [SerializeField]
        private Sprite selectedCursor;

        [SerializeField]
        private Sprite cursor;

        [SerializeField]
        private Image portraitImage;

        [Header("Unity Events")]

        [SerializeField]
        private UnityEvent onCharacterSelected;

        [SerializeField]
        private UnityEvent onCharacterUnselected;

        [SerializeField]
        private UnityEvent onHovered;

        [SerializeField]
        private UnityEvent onUnhovered;

        private void OnDisable()
        {
            gameDataSO.OnAllPlayersReady -= ReadyUp;
            gameDataSO.OnAllPlayersUnready -= UnreadyUp;
        }

        public void Init(CharacterSO character)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = character.CharacterDisplayName;
            portraitImage.sprite = character.CharacterPortrait;

            LayoutGroup = GetComponentInChildren<GridLayoutGroup>();
            Character = character;

            gameDataSO.OnAllPlayersReady += ReadyUp;
            gameDataSO.OnAllPlayersUnready += UnreadyUp;

            for (var i = 0; i < markers.Length; i++)
            {
                markers[i].color = gameDataSO.PlayerColors[i];
            }
        }

        public void ReadyUp()
        {
            col.enabled = false;
        }

        public void UnreadyUp()
        {
            col.enabled = true;
        }

        public override void OnClick(PlayerCursorController cursor)
        {
            CharacterSO character = gameDataSO.GetPlayerData(cursor.PlayerID).SelectedCharacter;

            // Unselect if pressed again
            if (character == Character)
            {
                Unselect(cursor);
                return;
            }

            if (character != null)
            {
                return;
            }

            cursor.BackAction = Unselect;
            cursor.Cursor.sprite = selectedCursor;

            markers[cursor.PlayerID].gameObject.SetActive(true);
            gameDataSO.SetPlayerSelectedCharacter(cursor.PlayerID, Character);

            onCharacterSelected?.Invoke();
        }

        private void RemovePlayer(PlayerCursorController cursor)
        {
            gameDataSO.RemovePlayer(cursor.PlayerID);
        }

        private void Unselect(PlayerCursorController cursor)
        {
            cursor.BackAction = RemovePlayer;
            cursor.Cursor.sprite = this.cursor;

            markers[cursor.PlayerID].gameObject.SetActive(false);
            gameDataSO.SetPlayerSelectedCharacter(cursor.PlayerID, null);
            gameDataSO.SetPlayerProspectCharacter(cursor.PlayerID, null); // this is stupid I'm sorry

            onCharacterUnselected?.Invoke();
        }

        public override void OnHoverEnter(PlayerCursorController cursor)
        {
            PlayerData playerData = gameDataSO.GetPlayerData(cursor.PlayerID);
            if (playerData.SelectedCharacter != null || playerData.ProspectCharacter == Character)
            {
                return;
            }

            gameDataSO.SetPlayerProspectCharacter(cursor.PlayerID, Character);
            onHovered?.Invoke();
        }

        public override void OnHoverExit(PlayerCursorController cursor)
        {
            if (gameDataSO.GetPlayerData(cursor.PlayerID).SelectedCharacter != null)
            {
                return;
            }

            gameDataSO.SetPlayerProspectCharacter(cursor.PlayerID, null);
            onUnhovered?.Invoke();
        }
    }
}