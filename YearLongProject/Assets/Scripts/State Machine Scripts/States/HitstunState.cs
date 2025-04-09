using EditorUtils.BoldHeader;
using GameEntities;
using Movement;
using NaughtyAttributes;
using Timeline;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    public class HitstunState : CharacterState
    {
        [BoldHeader("Hitstun State")]
        [InfoBox("State for when the character is hit and stunned.")]
        [Header("HitstunState Config")]

        [SerializeField]
        private CharacterEntity characterEntity;

        [SerializeField]
        private SimpleMovementController movementController;

        [SerializeField]
        private ManualTimelinePlayer hitstunPlayableAsset;

        [SerializeField]
        private StateNameSO jumpState;

        private void Update()
        {
            if (Time.time < characterEntity.StunTime)
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