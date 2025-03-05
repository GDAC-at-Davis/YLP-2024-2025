using Movement;
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
        private StateNameSO jumpState;

        private MoveSubStates currentSubState;
        private ManualTimelinePlayer currentPlayableAsset;

        private void Update()
        {
            Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
            movementController.SetCharacterMove(moveInput.x);
            SelectMoveState(moveInput);
            ActionManager.SetActionTypeAllowed(jumpState, movementController.GetIsGrounded());
        }

        private void FixedUpdate()
        {
            if (currentPlayableAsset != null)
            {
                currentPlayableAsset.Evaluate(ActionManager.FixedDeltaTime);
            }
        }

        protected override void OnEnable()
        {
            currentPlayableAsset = null;
            currentSubState = (MoveSubStates)(-1);
            SelectMoveState(Vector2.zero);
        }

        protected override void OnDisable()
        {
            currentPlayableAsset?.Stop();
            currentPlayableAsset = null;
        }

        private void SelectMoveState(Vector2 moveInput)
        {
            if (movementController.GetIsGrounded())
            {
                if (Mathf.Abs(characterRigidbody.LinearVelocity.x) < 0.5f && moveInput == Vector2.zero)
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