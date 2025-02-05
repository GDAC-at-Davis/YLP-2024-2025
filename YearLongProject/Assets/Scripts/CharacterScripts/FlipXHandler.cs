using UnityEngine;
using UnityEngine.Events;

namespace CharacterScripts
{
    public class FlipXHandler : MonoBehaviour
    {
        [SerializeField]
        private Character _character;

        [SerializeField]
        private PlayerInputSO _playerInputSO;

        public UnityEvent<bool> OnFlipXChange;

        public bool CanFlipX
        {
            get => _canFlipX;
            set => _canFlipX = value;
        }

        private bool _currentFlipX;
        private bool _canFlipX = true;

        private void Start()
        {
            _playerInputSO.MoveEvent(_character.EntityID) += HandleMoveInput;
        }

        private void OnDestroy()
        {
            _playerInputSO.MoveEvent(_character.EntityID) -= HandleMoveInput;
        }

        private void HandleMoveInput(Vector2 moveDir)
        {
            if (!_canFlipX)
            {
                return;
            }

            if (moveDir.x > 0)
            {
                if (_currentFlipX)
                {
                    _currentFlipX = false;
                    OnFlipXChange?.Invoke(_currentFlipX);
                }
            }
            else if (moveDir.x < 0)
            {
                if (!_currentFlipX)
                {
                    _currentFlipX = true;
                    OnFlipXChange?.Invoke(_currentFlipX);
                }
            }
        }
    }
}