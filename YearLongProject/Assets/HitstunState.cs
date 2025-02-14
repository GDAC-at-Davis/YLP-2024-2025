using Animancer;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    public class HitstunState : CharacterState
    {
        [Header("HitstunState Config")]

        [SerializeField]
        private SimpleMovementController movementController;

        [SerializeField]
        private PlayableAssetTransitionExt hitstunPlayableAsset;

        [SerializeField]
        private StateNameSO jumpState;

        private void Update()
        {
            if (Time.time < movementController.stunTime) return;

            ActionManager.SetAllActionTypeAllowed(true);
            ActionManager.StateMachine.TrySetDefaultState();
            ActionManager.SetActionTypeAllowed(jumpState, movementController.GetIsGrounded());
        }

        protected override void OnEnable()
        {
            Anim.Play(hitstunPlayableAsset);
            hitstunPlayableAsset.Events.OnEnd += HandleOnEnd;

            movementController.ApplyImpulseForce(movementController.Knockback);
            ActionManager.SetAllActionTypeAllowed(false);
        }

        protected override void OnDisable()
        {
            hitstunPlayableAsset.Events.OnEnd -= HandleOnEnd;
        }

        private void HandleOnEnd()
        {

        }
    }
}