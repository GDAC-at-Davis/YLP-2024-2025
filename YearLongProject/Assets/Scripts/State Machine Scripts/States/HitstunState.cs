using Movement;
using Timeline;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    public class HitstunState : CharacterState
    {
        [Header("HitstunState Config")]

        [SerializeField]
        private SimpleMovementController movementController;

        [SerializeField]
        private ManualTimelinePlayer hitstunPlayableAsset;

        [SerializeField]
        private StateNameSO jumpState;

        private void Update()
        {
            if (Time.time < movementController.stunTime)
            {
                return;
            }

            ActionManager.SetAllActionTypeAllowed(true);
            ActionManager.StateMachine.TrySetDefaultState();
            HandleOnEnd();
            ActionManager.SetActionTypeAllowed(jumpState, movementController.GetIsGrounded());
        }

        private void FixedUpdate()
        {
            hitstunPlayableAsset.Evaluate(ActionManager.FixedDeltaTime);
        }

        protected override void OnEnable()
        {
            hitstunPlayableAsset.Play();

            ActionManager.SetAllActionTypeAllowed(false);
            movementController.enabled = false;
        }

        protected override void OnDisable()
        {
            movementController.enabled = true;
        }

        private void HandleOnEnd()
        {
            hitstunPlayableAsset.Stop();
        }
    }
}