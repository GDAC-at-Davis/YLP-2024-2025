using Input_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterScripts
{
    public class FlipXHandler : MonoBehaviour
    {
        [SerializeField]
        private Character character;

        [SerializeField]
        private PlayerInputSo playerInputSo;

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
            playerInputSo.MoveEvent(character.PlayerId) += HandleMoveInput;
        }

        private void OnDestroy()
        {
            playerInputSo.MoveEvent(character.PlayerId) -= HandleMoveInput;
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