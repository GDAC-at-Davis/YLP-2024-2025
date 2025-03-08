using Movement;
using Timeline;
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

        [SerializeField]
        private ManualTimelinePlayer jumpPlayableAsset;

        public override bool CanEnterState
            => ActionManager.GetActionTypeAllowed(StateName) && movementController.GetIsGrounded();

        private float jumpTimer;

        private void Update()
        {
            Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
            movementController.SetHorizontalInput(moveInput.x);

            movementController.SetJumpVelocity(jumpVelocity * jumpMultCurve.Evaluate(jumpTimer / maxJumpDuration));

            jumpTimer += Time.deltaTime;

            if (!ActionManager.CharacterActionInput.JumpHeld || jumpTimer > maxJumpDuration)
            {
                movementController.StopJump();
                ActionManager.StateMachine.ForceSetDefaultState();
            }
        }

        private void FixedUpdate()
        {
            jumpPlayableAsset.Evaluate(ActionManager.FixedDeltaTime);
        }

        protected override void OnEnable()
        {
            movementController.StartJump();
            jumpTimer = 0;
            jumpPlayableAsset.Play();
        }

        protected override void OnDisable()
        {
            jumpPlayableAsset.Stop();
            movementController.StopJump();
        }
    }
}