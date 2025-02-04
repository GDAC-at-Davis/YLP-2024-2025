using UnityEngine;

public class MoveState : CharacterState
{
    private void Update()
    {
        Vector2 moveInput = actionManager.GetPlayerActionInput().moveDir;
        movementController.SetCharacterMove(moveInput.x);
    }

    protected override void OnEnable()
    {
    }

    protected override void OnDisable()
    {
    }
}