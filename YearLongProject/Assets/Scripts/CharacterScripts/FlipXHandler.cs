using Base;
using Input_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterScripts
{
    /// <summary>
    ///     Handles flipping the character based on input
    /// </summary>
    public class FlipXHandler : DescriptionMono
    {
        [SerializeField]
        private CharacterActionInput characterActionInput;

        public UnityEvent<bool> OnFlipXChange;

        public bool CurrentFlipX => currentFlipX;

        public bool CanFlipX
        {
            get => canFlipX;
            set => canFlipX = value;
        }

        private bool currentFlipX;
        private bool canFlipX = true;

        private void Start()
        {
            characterActionInput.MoveInputChanged += HandleMoveInput;
        }

        private void OnDestroy()
        {
            characterActionInput.MoveInputChanged -= HandleMoveInput;
        }

        private void HandleMoveInput(Vector2 moveDir)
        {
            if (!canFlipX)
            {
                return;
            }

            if (moveDir.x > 0)
            {
                if (currentFlipX)
                {
                    currentFlipX = false;
                    OnFlipXChange?.Invoke(currentFlipX);
                }
            }
            else if (moveDir.x < 0)
            {
                if (!currentFlipX)
                {
                    currentFlipX = true;
                    OnFlipXChange?.Invoke(currentFlipX);
                }
            }
        }
    }
}