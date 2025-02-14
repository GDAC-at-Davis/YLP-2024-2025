using Input_Scripts;
using TMPro;
using UnityEngine;

/// <summary>
/// Temporary Character Select UI Component
/// Tracks and relays ready status of player
/// </summary>
public class PlayerReadyButton : MonoBehaviour
{
    [SerializeField]
    PlayerInputSo playerInputSO;
    PlayerInputSo.PlayerInputEvents events;

    TextMeshProUGUI text;

    bool ready = false;
    int playerID;

    /// <summary>
    /// Known Issue:
    /// If first controller joins by pressing light attack button it instantly readies the player up and starting the game
    /// </summary>
    public void Initialize(int id)
    {
        playerID = id;
        events = playerInputSO.TryGetPlayerInputEvents(id);
        events.LightAttackEvent += ToggleReady;

        CharacterSelect.Instance.AllPlayersReady += LockIn;
        CharacterSelect.Instance.ReadyUp(playerID, false);
        
        text = GetComponent<TextMeshProUGUI>();
        text.text = $"Player {id}";
        text.color = Color.red;
    }

    private void OnDisable()
    {
        events.LightAttackEvent -= ToggleReady;
        CharacterSelect.Instance.AllPlayersReady -= LockIn;
    }

    void ToggleReady(bool pressed)
    {
        if (!pressed) return;

        ready = !ready;
        text.color = ready ? Color.green : Color.red;
        CharacterSelect.Instance.ReadyUp(playerID, ready);
    }

    void LockIn()
    {
        this.enabled = false;
    }
}
