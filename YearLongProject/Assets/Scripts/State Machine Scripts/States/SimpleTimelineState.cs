using EditorUtils.BoldHeader;
using Movement;
using NaughtyAttributes;
using Timeline;
using UnityEngine;

namespace State_Machine_Scripts.States
{
    /// <summary>
    ///     Bare bones state that plays a timeline and then returns to the default state.
    /// </summary>
    public class SimpleTimelineState : CharacterState
    {
        [BoldHeader("Simple Timeline State")]
        [InfoBox(
            "A state that simply plays a Timeline when entered, and returns to the default state when the Timeline ends.")]
        [Header("Dependencies")]

        [SerializeField]
        private SimpleMovementController movementController;

        [Header("Config")]

        [SerializeField]
        private ManualTimelinePlayer timelinePlayer;

        [SerializeField]
        private bool useDefaultMovement;

        private void Update()
        {
            if (useDefaultMovement)
            {
                Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
                movementController.SetHorizontalInput(moveInput.x);
            }
            else
            {
                movementController.SetHorizontalInput(0);
            }
        }

        private void FixedUpdate()
        {
            timelinePlayer.Evaluate(ActionManager.FixedDeltaTime);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timelinePlayer.OnFinished += HandleOnEnd;
            timelinePlayer.Play();
        }

        private void HandleOnEnd()
        {
            ActionManager.StateMachine.TrySetDefaultState();
        }

        public override void OnExitState()
        {
            base.OnExitState();

            timelinePlayer.OnFinished -= HandleOnEnd;
            timelinePlayer.Stop();

            ActionManager.SetAllActionTypeAllowed(true);
        }
    }
}