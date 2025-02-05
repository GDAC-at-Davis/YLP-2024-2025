using UnityEngine;

public class MoveState : CharacterState
{
    private void Update()
    {
        Vector2 moveInput = ActionManager.GetPlayerActionInput().MoveDir;
        MovementController.SetCharacterMove(moveInput.x);
    }

    protected override void OnEnable()
    {
    }

    protected override void OnDisable()
    {
    }
}