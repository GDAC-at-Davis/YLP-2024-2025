using Input_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterScripts
{
    public class FlipXHandler : MonoBehaviour
    {
        [SerializeField]
        private CharacterActionInput characterActionInput;

        public UnityEvent<bool> OnFlipXChange;

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