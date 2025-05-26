using CharacterScripts;
using Input_Scripts;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button that holds characterSO for character selection
/// </summary>
public class CharacterSelectButton : ButtonBehavior
{
    [SerializeField]
    GameDataSO gameDataSO;

    public CharacterSO Character;
    public GridLayoutGroup LayoutGroup;

    public void Init(CharacterSO character)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = character.CharacterDisplayName;
        GetComponent<Image>().sprite = character.CharacterPortrait;

        LayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        Character = character; 
    }

    public override void OnClick(PlayerCursorController cursor)
    {
        // Unselect if pressed again
        if (gameDataSO.GetPlayerData(cursor.PlayerID).SelectedCharacter == Character)
        {
            Unselect(cursor);
            return;
        }

        cursor.transform.parent = LayoutGroup.transform;
        cursor.SetText("");
        cursor.Selected = true;
        cursor.BackAction = Unselect;

        gameDataSO.SetPlayerSelectedCharacter(cursor.PlayerID, Character);
    }

    void RemovePlayer(PlayerCursorController cursor)
    {
        gameDataSO.RemovePlayer(cursor.PlayerID);
    }
    void Unselect(PlayerCursorController cursor)
    {
        cursor.SetText((cursor.PlayerID + 1).ToString());
        cursor.transform.parent = cursor.Container;
        cursor.Selected = false;
        cursor.BackAction = RemovePlayer;

        gameDataSO.SetPlayerSelectedCharacter(cursor.PlayerID, null);
    }

    public override void OnHoverEnter(PlayerCursorController cursor)
    {
        if (gameDataSO.GetPlayerData(cursor.PlayerID).ProspectCharacter == Character)
        {
            return;
        }

        gameDataSO.SetPlayerProspectCharacter(cursor.PlayerID, Character);
    }

    public override void OnHoverExit(PlayerCursorController cursor)
    {
        gameDataSO.SetPlayerProspectCharacter(cursor.PlayerID, null);
    }
}
