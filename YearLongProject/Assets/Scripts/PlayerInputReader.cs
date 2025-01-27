using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    int _id;
    [SerializeField] PlayerInputSO _playerInputSO;

    // Set by character select in the future
    public GameObject character;

    private void Start()
    {
        _id = GetInstanceID();
        Debug.Log(_id);

        // Run after character selected in the future
        Instantiate(character, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>().Init(_id);
    }

    public void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _playerInputSO.AttackEvent.Invoke(_id, context.action.triggered);
    }
    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _playerInputSO.MoveEvent(_id, context.ReadValue<Vector2>());
    }
    public void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _playerInputSO.JumpEvent(_id, context.action.triggered);
    }
}
