using Animancer;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    public class LightAttackState : CharacterState
    {
        [SerializeField]
        private AnimancerComponent animancer;

        [SerializeField]
        private PlayableAssetTransition lightAttackPlayableAsset;

        private void Update()
        {
            MovementController.SetCharacterMove(0);
        }

        public override void OnEnterState()
        {
            animancer.Play(lightAttackPlayableAsset);
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