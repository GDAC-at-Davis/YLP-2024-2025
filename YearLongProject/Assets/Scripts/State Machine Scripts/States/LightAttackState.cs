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
            MovementController.SetCharacterMove(0);
            if (playableDirector.state != PlayState.Playing)
            {
                ActionManager.StateMachine.TrySetDefaultState();
            }
        }

        public override void OnEnterState()
        {
            playableDirector.playableAsset = lightAttackPlayableAsset;
            playableDirector.Play();
            enabled = true;
            ActionManager.SetActionTypeAllowed(CharacterActionType.Jump, false);
        }

        public override void OnExitState()
        {
            playableDirector.Stop();
            enabled = false;
            ActionManager.SetActionTypeAllowed(CharacterActionType.Jump, true);
        }
    }
}