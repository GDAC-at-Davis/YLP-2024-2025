using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInputSO _playerInput;
    int _id;

    [SerializeField] Vector2 _moveInput;
    [SerializeField] bool _jump;
    [SerializeField] bool _attack;

    private void OnEnable()
    {
        _playerInput.MoveEvent += OnMove;
        _playerInput.JumpEvent += OnJump;
        _playerInput.AttackEvent += OnAttack;
    }
    private void OnDisable()
    {
        _playerInput.MoveEvent -= OnMove;
        _playerInput.JumpEvent -= OnJump;
        _playerInput.AttackEvent -= OnAttack;
    }

    public void Init(int id)
    {
        _id = id;
        transform.parent = null;
    }

    private void OnMove(int id, Vector2 moveInput)
    {
        if (id != _id) return;
        _moveInput = moveInput;
    }

    private void OnJump(int id, bool jumped)
    {
        if (id != _id) return;
        _jump = jumped;
    }
    private void OnAttack(int id, bool attacked)
    {
        if (id != _id) return;
        _attack = attacked;
    }
}
