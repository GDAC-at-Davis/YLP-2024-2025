using UnityEngine;

namespace State_Machine_Scripts.States
{
    public class JumpState : CharacterState
    {
        [SerializeField]
        private float jumpVelocity = 10;

        [SerializeField]
        private float maxJumpDuration = 2;

        [SerializeField]
        private SimpleMovementController movementController;

        [SerializeField]
        private AnimationCurve jumpMultCurve;

        public override bool CanEnterState
            => ActionManager.GetActionTypeAllowed(StateName) && movementController.GetIsGrounded();

        private float jumpTimer;

        private void Update()
        {
            Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
            movementController.SetCharacterMove(moveInput.x);

            movementController.SetJumpVelocity(jumpVelocity * jumpMultCurve.Evaluate(jumpTimer / maxJumpDuration));

            jumpTimer += Time.deltaTime;

            if (!ActionManager.CharacterActionInput.JumpHeld || jumpTimer > maxJumpDuration)
            {
                movementController.StopJump();
                ActionManager.StateMachine.ForceSetDefaultState();
            }
        }

        protected override void OnEnable()
        {
            movementController.StartJump();
            jumpTimer = 0;
        }

        protected override void OnDisable()
        {
            movementController.StopJump();
        }
    }
}