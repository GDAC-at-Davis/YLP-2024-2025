using CharacterScripts;
using Input_Scripts;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Managers.GameDataSO;

/// <summary>
/// Button that holds characterSO for character selection
/// </summary>
namespace Input_Scripts
{
    public class CharacterSelectButton : ButtonBehavior
    {
        [SerializeField]
        GameDataSO gameDataSO;

        public CharacterSO Character;
        public GridLayoutGroup LayoutGroup;

        [SerializeField]
        Image[] markers;

        [SerializeField]
        Sprite selectedCursor;
        [SerializeField]
        Sprite cursor;

        public void Init(CharacterSO character)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = character.CharacterDisplayName;
            GetComponent<Image>().sprite = character.CharacterPortrait;

            LayoutGroup = GetComponentInChildren<GridLayoutGroup>();
            Character = character;

            gameDataSO.OnAllPlayersReady += ReadyUp;
            gameDataSO.OnAllPlayersUnready += UnreadyUp;
            
            for (int i = 0; i < markers.Length; i++)
            {
                markers[i].color = gameDataSO.PlayerColors[i];
            }
        }

        private void OnDisable()
        {
            gameDataSO.OnAllPlayersReady -= ReadyUp;
            gameDataSO.OnAllPlayersUnready -= UnreadyUp;
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
        }

        void RemovePlayer(PlayerCursorController cursor)
        {
            gameDataSO.RemovePlayer(cursor.PlayerID);
        }
        void Unselect(PlayerCursorController cursor)
        {
            cursor.BackAction = RemovePlayer;
            cursor.Cursor.sprite = this.cursor;

            markers[cursor.PlayerID].gameObject.SetActive(false);
            gameDataSO.SetPlayerSelectedCharacter(cursor.PlayerID, null);
            gameDataSO.SetPlayerProspectCharacter(cursor.PlayerID, null); // this is stupid I'm sorry
        }

        public override void OnHoverEnter(PlayerCursorController cursor)
        {
            PlayerData playerData = gameDataSO.GetPlayerData(cursor.PlayerID);
            if (playerData.SelectedCharacter != null || playerData.ProspectCharacter == Character)
            {
                return;
            }

            gameDataSO.SetPlayerProspectCharacter(cursor.PlayerID, Character);
        }

        public override void OnHoverExit(PlayerCursorController cursor)
        {
            if (gameDataSO.GetPlayerData(cursor.PlayerID).SelectedCharacter != null)
            {
                return;
            }
            gameDataSO.SetPlayerProspectCharacter(cursor.PlayerID, null);
        }
    }
}
