using UnityEngine;

public class JumpState : CharacterState
{
    [SerializeField] 
    private float jumpVelocity = 10;

    [SerializeField]
    private float maxJumpDuration = 2;

    [SerializeField]
    private AnimationCurve jumpMultCurve;

    private float jumpTimer;

    public override bool CanEnterState
    => actionManager.allowedActionTypes[actionType] && movementController.GetIsGrounded();

    private void Update()
    {
        Vector2 moveInput = actionManager.GetPlayerActionInput().moveDir;
        movementController.SetCharacterMove(moveInput.x);

        movementController.SetJumpVelocity(jumpVelocity * jumpMultCurve.Evaluate(jumpTimer/maxJumpDuration));

        jumpTimer += Time.deltaTime;

        if (!actionManager.GetPlayerActionInput().jumpHeld || jumpTimer > maxJumpDuration)
        {
            movementController.StopJump();
            actionManager.stateMachine.ForceSetDefaultState();
        }
    }

    protected override void OnEnable()
    {
        movementController.StartJump();
        jumpTimer = 0;
    }

    protected override void OnDisable()
    {
    }
}