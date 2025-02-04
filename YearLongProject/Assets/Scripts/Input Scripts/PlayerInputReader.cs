using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    int _id;
    UnityEngine.InputSystem.PlayerInput _playerInput;
    [SerializeField] PlayerInputSO _playerInputSO;

    // Set by character select in the future
    public GameObject character;

    private void Start()
    {
        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _id = _playerInput.playerIndex;

        // This is technically a bad thing to do with ScriptableObjects but whatever
        _playerInputSO.AddInputReader(_id);

        // Run by character selection in the future
        Instantiate(character, Vector3.zero, Quaternion.identity).GetComponent<Character>().Init(_id);
    }

    private void OnDestroy()
    {
        _playerInputSO.RemoveInputReader(_id);
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        _playerInputSO.LightAttackEvent(_id)?.Invoke(context.action.triggered);
    }
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        _playerInputSO.HeavyAttackEvent(_id)?.Invoke(context.action.triggered);
    }
    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        _playerInputSO.SpecialAttackEvent(_id)?.Invoke(context.action.triggered);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _playerInputSO.MoveEvent(_id)?.Invoke(context.ReadValue<Vector2>());
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        _playerInputSO.JumpEvent(_id)?.Invoke(context.action.triggered);
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        _playerInputSO.DashEvent(_id)?.Invoke(context.action.triggered);
    }
}
