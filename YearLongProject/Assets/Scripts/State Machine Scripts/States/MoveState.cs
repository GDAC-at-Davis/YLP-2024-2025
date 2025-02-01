using UnityEngine;

public class MoveState : CharacterState
{
    [SerializeField]
    private PlayerController _playerController;

    private bool _inState;

    private void Update()
    {
        if (!_inState)
        {
            return;
        }

        Vector2 moveInput = _playerController.MoveInput;
        movementController.CharacterMove(moveInput);
    }

    public override void OnEnterState()
    {
        _inState = true;
        base.OnEnterState();
    }

    public override void OnExitState()
    {
        _inState = false;
        base.OnExitState();
    }
}