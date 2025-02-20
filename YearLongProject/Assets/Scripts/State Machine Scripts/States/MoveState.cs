using Animancer;
using Movement;
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
        private PlayableAssetTransitionExt movingPlayableAsset;

        [SerializeField]
        private PlayableAssetTransitionExt idlePlayableAsset;

        [SerializeField]
        private PlayableAssetTransitionExt airPlayableAsset;

        [SerializeField]
        private StateNameSO jumpState;

        private MoveSubStates currentSubState;
        private PlayableAssetTransitionExt currentPlayableAsset;

        private void Update()
        {
            Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
            movementController.SetCharacterMove(moveInput.x);
            SelectMoveState(moveInput);
            ActionManager.SetActionTypeAllowed(jumpState, movementController.GetIsGrounded());
        }

        protected override void OnEnable()
        {
            SelectMoveState(Vector2.zero);
        }

        protected override void OnDisable()
        {
            currentPlayableAsset.Events.OnEnd -= HandleOnEnd;
            currentSubState = MoveSubStates.Idle;
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

            if (currentPlayableAsset != null)
            {
                currentPlayableAsset.Events.OnEnd -= HandleOnEnd;
            }

            currentSubState = subState;

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
            }

            Anim.Play(currentPlayableAsset);
            currentPlayableAsset.Events.OnEnd += HandleOnEnd;
        }

        private void HandleOnEnd()
        {
            // Only way to loop timelines
            // https://discussions.unity.com/t/animancer-less-animator-controller-more-animator-control/717489/868?page=44
            Anim.Play(currentPlayableAsset).Time = 0;
        }
    }
}