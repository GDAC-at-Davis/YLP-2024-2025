using Animancer;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    /// <summary>
    ///     Bare bones state that plays a timeline and then returns to the default state.
    /// </summary>
    public class SimpleTimelineState : CharacterState
    {
        [Header("Config")]

        [SerializeField]
        private SimpleMovementController movementController;

        [SerializeField]
        private PlayableAssetTransition lightAttackPlayableAsset;

        [SerializeField]
        private bool useDefaultMovement;

        [SerializeField]
        private StateNameSO jumpState;

        private void Update()
        {
            if (useDefaultMovement)
            {
                Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
                movementController.SetCharacterMove(moveInput.x);
            }
            else
            {
                movementController.SetCharacterMove(0);
            }
        }

        public override void OnEnterState()
        {
            Anim.Play(lightAttackPlayableAsset);
            lightAttackPlayableAsset.Events.OnEnd += HandleOnEnd;
            ActionManager.SetActionTypeAllowed(jumpState, false);
        }

        private void HandleOnEnd()
        {
            ActionManager.StateMachine.TrySetDefaultState();
        }

        public override void OnExitState()
        {
            lightAttackPlayableAsset.Events.OnEnd -= HandleOnEnd;
            ActionManager.SetActionTypeAllowed(jumpState, true);
        }
    }
}