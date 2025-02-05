using UnityEngine;
using UnityEngine.Playables;

namespace State_Machine_Scripts.States
{
    public class LightAttackState : CharacterState
    {
        [SerializeField]
        private PlayableDirector _playableDirector;

        [SerializeField]
        private PlayableAsset _lightAttackPlayableAsset;

        private void Update()
        {
            movementController.SetCharacterMove(0);
            if (_playableDirector.state != PlayState.Playing)
            {
                actionManager.stateMachine.TrySetDefaultState();
            }
        }

        public override void OnEnterState()
        {
            _playableDirector.playableAsset = _lightAttackPlayableAsset;
            _playableDirector.Play();
            enabled = true;
        }

        public override void OnExitState()
        {
            _playableDirector.Stop();
            enabled = false;
        }
    }
}