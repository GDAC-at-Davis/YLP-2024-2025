using EditorUtils.BoldHeader;
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
        private bool CanFlipX => flipXCounter <= 0;

        private int flipXCounter;

        private bool targetFlipX;
        private bool currentFlipX;

        private void Start()
        {
            characterActionInput.MoveInputChanged += HandleMoveInput;
        }

        private void FixedUpdate()
        {
            if (!CanFlipX)
            {
                return;
            }

            SyncFlipX();
        }

        private void OnDestroy()
        {
            characterActionInput.MoveInputChanged -= HandleMoveInput;
        }

        public void AddLockOnFlipX()
        {
            flipXCounter++;
        }

        public void RemoveLockOnFlipX()
        {
            flipXCounter--;

            if (flipXCounter < 0)
            {
                flipXCounter = 0;
            }
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

        /// <summary>
        ///     Apply the target/desired flipX to the character
        /// </summary>
        private void SyncFlipX()
        {
            if (currentFlipX == targetFlipX)
            {
                return;
            }

            currentFlipX = targetFlipX;
            OnFlipXChange?.Invoke(currentFlipX);
        }

        public void ForceSetFlipX(bool flipX)
        {
            targetFlipX = flipX;
            SyncFlipX();
        }
    }
}