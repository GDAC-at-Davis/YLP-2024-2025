using UnityEngine;

public class JumpState : CharacterState
{
    [SerializeField]
    private float jumpVelocity = 10;

    [SerializeField]
    private float maxJumpDuration = 2;

    [SerializeField]
    private AnimationCurve jumpMultCurve;

    public override bool CanEnterState
        => ActionManager.GetActionTypeAllowed(ActionType) && MovementController.GetIsGrounded();

    private float jumpTimer;

    private void Awake()
    {
        ActionType = CharacterActionType.Jump;
    }

    private void Update()
    {
        Vector2 moveInput = ActionManager.GetPlayerActionInput().MoveDir;
        MovementController.SetCharacterMove(moveInput.x);

        MovementController.SetJumpVelocity(jumpVelocity * jumpMultCurve.Evaluate(jumpTimer / maxJumpDuration));

        jumpTimer += Time.deltaTime;

        if (!ActionManager.GetPlayerActionInput().JumpHeld || jumpTimer > maxJumpDuration)
        {
            MovementController.StopJump();
            ActionManager.StateMachine.ForceSetDefaultState();
        }
    }

    protected override void OnEnable()
    {
        MovementController.StartJump();
        jumpTimer = 0;
    }

    protected override void OnDisable()
    {
        MovementController.StopJump();
    }
}