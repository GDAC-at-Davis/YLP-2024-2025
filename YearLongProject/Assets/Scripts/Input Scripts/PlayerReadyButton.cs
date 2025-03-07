using CharacterScripts;
using Input_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Temporary Character Select cursor thing its 2:30 am and I can't figure out how to combine
/// the virtual mouse and multiplayer event system so we're gonna do whatever the fuck this is for now 
/// </summary>
public class PlayerReadyController : MonoBehaviour
{
    [SerializeField]
    PlayerInputSo playerInputSO;

    [SerializeField]
    float speed = 500;

    PlayerInputSo.PlayerInputEvents events;

    int playerID;
    Vector3 input;

    public void Initialize(int id)
    {
        playerID = id;
        GetComponentInChildren<TextMeshProUGUI>().text = (id + 1).ToString();

        events = playerInputSO.TryGetPlayerInputEvents(id);
        events.LightAttackEvent += TrySelectCharacter;
        events.HeavyAttackEvent += UnselectCharacter;
        events.MoveEvent += MoveCursor;

        CharacterSelect.Instance.AllPlayersReady += LockIn;
        CharacterSelect.Instance.ReadyUp(playerID, null);
    }

    private void Update()
    {
        if (input == Vector3.zero) return;

        transform.position += input * Time.deltaTime * speed;
    }

    private void MoveCursor(Vector2 input)
    {
        this.input = (Vector3)input;
    }

    private void OnDisable()
    {
        events.LightAttackEvent -= TrySelectCharacter;
        events.HeavyAttackEvent -= UnselectCharacter;
        events.MoveEvent -= MoveCursor;
        CharacterSelect.Instance.AllPlayersReady -= LockIn;
    }

    void TrySelectCharacter(bool pressed)
    {
        if (!pressed) return;
        CharacterSO character = Physics2D.OverlapPoint(transform.position).GetComponent<CharacterSelectButton>().Character;
        CharacterSelect.Instance.ReadyUp(playerID, character);
    }

    void UnselectCharacter(bool pressed)
    {
        if (!pressed) return;
        CharacterSelect.Instance.ReadyUp(playerID, null);
    }

    void LockIn()
    {
        this.enabled = false;
    }
}
