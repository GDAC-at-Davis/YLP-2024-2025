using CharacterScripts;
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
        private CharacterFacingDirection characterFacingDirection;

        [Header("Config")]

        [SerializeField]
        private bool invertFacingDirection;

        /// <summary>
        ///     Saves knockback to apply when hitStun state is entered
        /// </summary>
        private Vector2 knockback;

        private float timer;

        private void FixedUpdate()
        {
            hitstunPlayableAsset.Evaluate(ActionManager.FixedDeltaTime);

            timer += ActionManager.FixedDeltaTime;
            if (timer < characterEntity.StunDuration)
            {
                return;
            }

            // Make sure to end the timeline before switching states
            HandleOnEnd();
            ActionManager.StateMachine.TrySetDefaultState();
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            timer = 0f;

            hitstunPlayableAsset.Play();

            movementController.enabled = false;

            movementController.SetVelocity(knockback);

            // Flip X
            if (knockback.x < 0)
            {
                characterFacingDirection.ForceSetFlipX(invertFacingDirection);
            }
            else if (knockback.x > 0)
            {
                characterFacingDirection.ForceSetFlipX(!invertFacingDirection);
            }
        }

        public override void OnExitState()
        {
            base.OnExitState();
            movementController.enabled = true;
        }

        private void HandleOnEnd()
        {
            hitstunPlayableAsset.Stop();
        }

        public void SetKnockback(Vector2 knockbackToCache)
        {
            knockback = knockbackToCache;
        }
    }
}