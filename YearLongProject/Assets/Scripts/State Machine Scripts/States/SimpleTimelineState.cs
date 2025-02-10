using Animancer;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    /// <summary>
    ///     Bare bones state that plays a timeline and then returns to the default state.
    /// </summary>
    public class SimpleTimelineState : CharacterState
    {
        [SerializeField]
        private SimpleMovementController movementController;

        [SerializeField]
        private PlayableAssetTransition lightAttackPlayableAsset;

        private void Update()
        {
            movementController.SetCharacterMove(0);
        }

        public override void OnEnterState()
        {
            Anim.Play(lightAttackPlayableAsset);
            lightAttackPlayableAsset.Events.OnEnd += HandleOnEnd;
            ActionManager.SetActionTypeAllowed(CharacterActionType.Jump, false);
        }

        private void HandleOnEnd()
        {
            ActionManager.StateMachine.TrySetDefaultState();
        }

        public override void OnExitState()
        {
            lightAttackPlayableAsset.Events.OnEnd -= HandleOnEnd;
            ActionManager.SetActionTypeAllowed(CharacterActionType.Jump, true);
        }
    }
}