using CharacterScripts;
using EditorUtils.BoldHeader;
using Movement;
using NaughtyAttributes;
using Timeline;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    public class MoveState : CharacterState
    {
        private enum MoveSubStates
        {
            Idle,
            Moving,
            Airborne
        }

        [BoldHeader("Basic Movement State")]
        [InfoBox("Basic movement state for running, jumping, and air.")]
        [Header("MoveState Config")]

        [SerializeField]
        private SimpleMovementController movementController;

        [SerializeField]
        private CharacterRigidbody2D characterRigidbody;

        [SerializeField]
        private ManualTimelinePlayer movingPlayableAsset;

        [SerializeField]
        private ManualTimelinePlayer idlePlayableAsset;

        [SerializeField]
        private ManualTimelinePlayer airPlayableAsset;

        [SerializeField]
        private CharacterState jumpState;

        [SerializeField]
        private CharacterFacingDirection characterFacingDirection;

        private MoveSubStates currentSubState;
        private ManualTimelinePlayer currentPlayableAsset;
        private bool canJump;

        private void Start()
        {
            if (jumpState)
            {
                jumpState.AddLockout();
            }
        }

        private void Update()
        {
            Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
            movementController.SetHorizontalInput(moveInput.x);
            SelectMoveState(moveInput);
            bool canJumpPrev = canJump;
            canJump = movementController.GetIsGrounded();

            if (jumpState)
            {
                if (!canJumpPrev && canJump)
                {
                    jumpState.RemoveLockout();
                }
                else if (canJumpPrev && !canJump)
                {
                    jumpState.AddLockout();
                }
            }
        }

        private void FixedUpdate()
        {
            if (currentPlayableAsset != null)
            {
                currentPlayableAsset.Evaluate(ActionManager.FixedDeltaTime);
                characterFacingDirection.SyncFlipX();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            currentPlayableAsset = null;
            currentSubState = (MoveSubStates)(-1);
            SelectMoveState(Vector2.zero);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            currentPlayableAsset?.Stop();
            currentPlayableAsset = null;
        }

        private void SelectMoveState(Vector2 moveInput)
        {
            if (movementController.GetIsGrounded())
            {
                if (Mathf.Abs(characterRigidbody.LinearVelocity.x) < 0.5f && moveInput.x == 0)
                {
                    SetSubState(MoveSubStates.Idle);
                }
                else
                {
                    SetSubState(MoveSubStates.Moving);
                }
            }
            else
            {
                SetSubState(MoveSubStates.Airborne);
            }
        }

        private void SetSubState(MoveSubStates subState)
        {
            if (currentSubState == subState)
            {
                return;
            }

            currentSubState = subState;

            currentPlayableAsset?.Stop();

            switch (subState)
            {
                case MoveSubStates.Idle:
                    currentPlayableAsset = idlePlayableAsset;
                    break;
                case MoveSubStates.Moving:
                    currentPlayableAsset = movingPlayableAsset;
                    break;
                case MoveSubStates.Airborne:
                    currentPlayableAsset = airPlayableAsset;
                    break;
                default:
                    Debug.LogError("Invalid MoveSubState");
                    break;
            }

            currentPlayableAsset.Play();
        }
    }
}