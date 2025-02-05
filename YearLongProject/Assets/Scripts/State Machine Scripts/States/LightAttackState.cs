using UnityEngine;
using UnityEngine.Playables;

namespace State_Machine_Scripts.States
{
    public class LightAttackState : CharacterState
    {
        [SerializeField]
        private PlayableDirector playableDirector;

        [SerializeField]
        private PlayableAsset lightAttackPlayableAsset;

        private void Update()
        {
            movementController.SetCharacterMove(0);
            if (playableDirector.state != PlayState.Playing)
            {
                actionManager.stateMachine.TrySetDefaultState();
            }
        }

        public override void OnEnterState()
        {
            playableDirector.playableAsset = lightAttackPlayableAsset;
            playableDirector.Play();
            enabled = true;
            actionManager.SetActionTypeAllowed(CharacterActionType.Jump, false);
        }

        public override void OnExitState()
        {
            playableDirector.Stop();
            enabled = false;
            actionManager.SetActionTypeAllowed(CharacterActionType.Jump, true);
        }
    }
}