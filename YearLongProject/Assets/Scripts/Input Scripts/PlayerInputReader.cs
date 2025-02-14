using GameEntities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Scripts
{
    /// <summary>
    ///     Mediator between the Input System and the PlayerInputSO
    /// </summary>
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputSo playerInputSo;

        // Set by character select in the future

        public GameObject Character;
        private int id;
        private UnityEngine.InputSystem.PlayerInput playerInput;

        private void Start()
        {
            playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            id = playerInput.playerIndex;

            playerInputSo.TryGetPlayerInputEvents(id);

            DontDestroyOnLoad(gameObject);

            // Run by character selection in the future
            //Instantiate(Character, Vector3.zero, Quaternion.identity).GetComponent<CharacterEntity>().Initialize(id);
        }

        private void OnDestroy()
        {
            playerInputSo.RemoveInputReader(id);
        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {
            playerInputSo.LightAttackEvent(id)?.Invoke(context.action.triggered);
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            playerInputSo.HeavyAttackEvent(id)?.Invoke(context.action.triggered);
        }

        public void OnSpecialAttack(InputAction.CallbackContext context)
        {
            playerInputSo.SpecialAttackEvent(id)?.Invoke(context.action.triggered);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            playerInputSo.MoveEvent(id)?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            playerInputSo.JumpEvent(id)?.Invoke(context.action.triggered);
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            playerInputSo.DashEvent(id)?.Invoke(context.action.triggered);
        }
    }
}