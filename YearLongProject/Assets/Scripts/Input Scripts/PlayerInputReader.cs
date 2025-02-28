using System.Linq;
using Base;
using GameEntities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Scripts
{
    /// <summary>
    ///     Mediator between the Input System and the PlayerInputSO
    /// </summary>
    public class PlayerInputReader : DescriptionMono
    {
        [SerializeField]
        private PlayerInputSo playerInputSo;

        [Header("Dev Tool")]

        [SerializeField]
        [Tooltip("Search for an existing character in the scene and link to that")]
        private bool quickLoad;

        private int id;
        private UnityEngine.InputSystem.PlayerInput playerInput;

        private void Start()
        {
            playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            id = playerInput.playerIndex;

            playerInputSo.TryGetPlayerInputEvents(id);

            if (!quickLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            if (quickLoad)
            {
                QuickLinkToExistingCharacter();
            }
        }

        private void OnDestroy()
        {
            playerInputSo.RemoveInputReader(id);
        }

        private void QuickLinkToExistingCharacter()
        {
            CharacterEntity character =
                FindObjectsByType<CharacterEntity>(FindObjectsSortMode.None)
                    .OrderBy(a => a.transform.GetSiblingIndex())
                    .First(c => !c.Initialized);

            if (character == null)
            {
                Debug.LogError("No character found in scene for quick linking input");
                return;
            }

            character.Initialize(id);
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