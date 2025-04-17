using System.Linq;
using Base;
using GameEntities;
using Managers;
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

        [SerializeField]
        private GameDataSO gameDataSo;

        [Header("Dev Tool")]

        [SerializeField]
        [Tooltip("Search for an existing character in the scene and link to that")]
        private bool quickLoad;

        private int playerId;
        private int inputId;

        private UnityEngine.InputSystem.PlayerInput playerInput;

        private void Start()
        {
            playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            inputId = playerInput.playerIndex;

            playerId = -1;

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
            if (playerId == -1)
            {
                return;
            }

            gameDataSo.RemovePlayer(playerId);
            playerInputSo.RemoveInputReader(playerId);
        }

        private void QuickLinkToExistingCharacter()
        {
            TryBindToPlayer();

            CharacterEntity character =
                FindObjectsByType<CharacterEntity>(FindObjectsSortMode.None)
                    .OrderBy(a => a.transform.GetSiblingIndex())
                    .First(c => !c.Initialized);

            if (character == null)
            {
                Debug.LogError("No character found in scene for quick linking input");
                return;
            }

            character.Initialize(playerId);
        }

        private void TryBindToPlayer()
        {
            int id = gameDataSo.TryAddPlayer();
            if (id != -1)
            {
                playerId = id;
            }
        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {
            if (playerId == -1)
            {
                return;
            }

            playerInputSo.LightAttackEvent(playerId)?.Invoke(context.action.triggered);
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (playerId == -1)
            {
                return;
            }

            playerInputSo.HeavyAttackEvent(playerId)?.Invoke(context.action.triggered);
        }

        public void OnSpecialAttack(InputAction.CallbackContext context)
        {
            if (playerId == -1)
            {
                return;
            }

            playerInputSo.SpecialAttackEvent(playerId)?.Invoke(context.action.triggered);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (playerId == -1)
            {
                return;
            }

            playerInputSo.MoveEvent(playerId)?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (playerId == -1)
            {
                TryBindToPlayer();
            }

            if (playerId == -1)
            {
                return;
            }

            playerInputSo.JumpEvent(playerId)?.Invoke(context.action.triggered);
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (playerId == -1)
            {
                return;
            }

            playerInputSo.DashEvent(playerId)?.Invoke(context.action.triggered);
        }
    }
}