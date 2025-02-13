using Animancer;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    public class MoveState : CharacterState
    {
        [Header("MoveState Config")]

        [SerializeField]
        private SimpleMovementController movementController;

        [SerializeField]
        private PlayableAssetTransitionExt movementPlayableAsset;

        [SerializeField]
        private StateNameSO jumpState;

        private void Update()
        {
            Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
            movementController.SetCharacterMove(moveInput.x);

            ActionManager.SetActionTypeAllowed(jumpState, movementController.GetIsGrounded());
        }

        protected override void OnEnable()
        {
            Anim.Play(movementPlayableAsset);
            movementPlayableAsset.Events.OnEnd += HandleOnEnd;
        }

        protected override void OnDisable()
        {
            movementPlayableAsset.Events.OnEnd -= HandleOnEnd;

            ActionManager.SetActionTypeAllowed(jumpState, true);
        }

        private void HandleOnEnd()
        {
            // Only way to loop timelines
            // https://discussions.unity.com/t/animancer-less-animator-controller-more-animator-control/717489/868?page=44
            Anim.Play(movementPlayableAsset).Time = 0;
        }
    }
}