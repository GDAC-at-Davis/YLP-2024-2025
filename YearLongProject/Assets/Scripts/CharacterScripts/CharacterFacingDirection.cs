using EditorUtils.BoldHeader;
using Hitbox.DataStructures;
using Hitbox.System;
using Input_Scripts;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterScripts
{
    /// <summary>
    ///     Handles flipping the character based on input
    /// </summary>
    public class CharacterFacingDirection : MonoBehaviour
    {
        [BoldHeader("Character Facing Direction Script")]
        [InfoBox("Handles information about the character's facing direction. Don't remove!")]
        [Header("Dependencies")]

        [SerializeField]
        private CharacterActionInput characterActionInput;

        [Header("Events")]

        [InfoBox("Add listeners to this UnityEvent to define custom behavior when the character's direction changes. " +
                 "\n'True' means the character is facing left \n'False' means the character is facing right.")]
        public UnityEvent<bool> OnFlipXChange;

        public bool CurrentFlipX => currentFlipX;

        public bool CanFlipX
        {
            get => canFlipX;
            set => canFlipX = value;
        }

        private bool targetFlipX;
        private bool currentFlipX;
        private bool canFlipX = true;

        private void Start()
        {
            characterActionInput.MoveInputChanged += HandleMoveInput;
        }

        private void FixedUpdate()
        {
            if (!canFlipX)
            {
                return;
            }

            SyncFlipX();
        }

        private void OnDestroy()
        {
            characterActionInput.MoveInputChanged -= HandleMoveInput;
        }

        private void HandleMoveInput(Vector2 moveDir)
        {
            if (moveDir.x > 0)
            {
                if (targetFlipX)
                {
                    targetFlipX = false;
                }
            }
            else if (moveDir.x < 0)
            {
                if (!targetFlipX)
                {
                    targetFlipX = true;
                }
            }
        }

        private void SyncFlipX()
        {
            if (currentFlipX == targetFlipX)
            {
                return;
            }

            currentFlipX = targetFlipX;
            OnFlipXChange?.Invoke(currentFlipX);
        }

        public void HandleOnHit(HitboxInstance instance, HitImpact impact)
        {
            Vector2 knockback = instance.CalculatedKnockback();

            if (knockback.x < 0)
            {
                targetFlipX = false;
            }
            else if (knockback.x > 0)
            {
                targetFlipX = true;
            }

            SyncFlipX();
        }
    }
}