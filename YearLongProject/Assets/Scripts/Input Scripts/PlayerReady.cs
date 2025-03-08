using UnityEngine;

namespace Input_Scripts
{
    /// <summary>
    ///     Temporary character select UI component
    ///     Spawns PlayerReadyController when a controller is conencted
    /// </summary>
    public class PlayerReady : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerReady;

        [SerializeField]
        private PlayerInputSo playerInputSO;

        [SerializeField]
        private RectTransform cursorBottomLeft;

        [SerializeField]
        private RectTransform cursorTopRight;

        private void OnEnable()
        {
            playerInputSO.ClearAllInputReaders();
            playerInputSO.PlayerInputAdded += OnInputAdded;
        }

        private void OnDisable()
        {
            playerInputSO.PlayerInputAdded -= OnInputAdded;
        }

        private void OnInputAdded(int id)
        {
            Debug.Log($"Player {id} connected");
            var button = Instantiate(playerReady, transform).GetComponent<PlayerReadyCursorController>();
            button.Initialize(id, cursorBottomLeft, cursorTopRight);
        }
    }
}