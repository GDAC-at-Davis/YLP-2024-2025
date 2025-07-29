using System.Collections.Generic;
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
        private class InputPlayerPairing
        {
            /// <summary>
            ///     Handles specific case of multiple players on the same input device (i.e shared keyboard)
            /// </summary>
            public int PairingId;

            public int PlayerId;
        }

        [SerializeField]
        private GameDataSO gameDataSo;

        [Header("Dev Tool")]

        [SerializeField]
        [Tooltip("Search for an existing character in the scene and link to that")]
        private bool quickLoad;

        [SerializeField]
        private PlayerInputSo playerInputSo;

        private readonly List<InputPlayerPairing> playerInputPairings = new();

        private int inputDeviceId;

        private UnityEngine.InputSystem.PlayerInput playerInput;

        private void Start()
        {
            playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            inputDeviceId = playerInput.playerIndex;

            if (!quickLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            if (quickLoad)
            {
                QuickLinkToExistingCharacter();
            }

            gameDataSo.OnPlayerDataChanged += HandlePlayerDataChanged;
        }

        private void OnDestroy()
        {
            if (playerInputPairings.Count == 0)
            {
                return;
            }

            InputPlayerPairing[] pairings = playerInputPairings.ToArray();
            foreach (InputPlayerPairing pairing in pairings)
            {
                gameDataSo.RemovePlayer(pairing.PlayerId);
            }
        }

        private int PairingIndexToPlayerId(int pairingIndex)
        {
            InputPlayerPairing pairing = playerInputPairings.FirstOrDefault(a => a.PairingId == pairingIndex);

            if (pairing == null)
            {
                return -1;
            }

            return pairing.PlayerId;
        }

        private bool IsValidPairing(int pairingIndex)
        {
            return playerInputPairings.FirstOrDefault(a => a.PairingId == pairingIndex) != null;
        }

        private void QuickLinkToExistingCharacter()
        {
            TryAddNewPlayer(0);

            CharacterEntity character =
                FindObjectsByType<CharacterEntity>(FindObjectsSortMode.None)
                    .OrderBy(a => a.transform.GetSiblingIndex())
                    .First(c => !c.Initialized);

            if (character == null)
            {
                Debug.LogError("No character found in scene for quick linking input");
                return;
            }

            character.Initialize(playerInputPairings[0].PlayerId);
        }

        private void TryAddNewPlayer(int pairingId)
        {
            // Only allow joining in character select
            if (!CharacterSelectCursorSpawner.InCharacterSelect && !quickLoad)
            {
                return;
            }

            // See if the pairing already exists
            if (playerInputPairings.FirstOrDefault(a => a.PairingId == pairingId) != null)
            {
                return;
            }

            // Create a new player and pair to it
            int id = gameDataSo.TryAddPlayer();
            if (id != -1)
            {
                // Pairing ID is so an input reader can have multiple players (e.g split keyboard)
                playerInputPairings.Add(new InputPlayerPairing
                {
                    PairingId = pairingId,
                    PlayerId = id
                });
            }
        }

        private void HandlePlayerDataChanged(int priorId, PlayerDataChange changetype,
            GameDataSO.PlayerData postchangedata)
        {
            for (var i = 0; i < playerInputPairings.Count; i++)
            {
                InputPlayerPairing pairing = playerInputPairings[i];
                int playerId = pairing.PlayerId;

                if (playerId != priorId)
                {
                    continue;
                }

                if (changetype == PlayerDataChange.PlayerRemoved)
                {
                    // The player was removed, so remove the pairing here
                    Debug.Log($"Removing player {playerId} pairing from input reader");
                    playerInputPairings.Remove(pairing);
                    i--;
                }
            }
        }

        private void DoLightAttack(int pairingIndex, bool value)
        {
            if (IsValidPairing(pairingIndex) == false)
            {
                return;
            }

            int playerId = PairingIndexToPlayerId(pairingIndex);
            playerInputSo.LightAttackEvent(playerId)?.Invoke(value);
        }

        public void OnLightAttackP1(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            DoLightAttack(0, context.ReadValueAsButton());
        }

        public void OnLightAttackP2(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            DoLightAttack(1, context.ReadValueAsButton());
        }

        private void DoHeavyAttack(int pairingIndex, bool value)
        {
            if (IsValidPairing(pairingIndex) == false)
            {
                return;
            }

            int playerId = PairingIndexToPlayerId(pairingIndex);
            playerInputSo.HeavyAttackEvent(playerId)?.Invoke(value);
        }

        public void OnHeavyAttackP1(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            DoHeavyAttack(0, context.ReadValueAsButton());
        }

        public void OnHeavyAttackP2(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            DoHeavyAttack(1, context.ReadValueAsButton());
        }

        private void DoSpecialAttack(int pairingIndex, bool value)
        {
            if (IsValidPairing(pairingIndex) == false)
            {
                return;
            }

            int playerId = PairingIndexToPlayerId(pairingIndex);
            playerInputSo.SpecialAttackEvent(playerId)?.Invoke(value);
        }

        public void OnSpecialAttackP1(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            DoSpecialAttack(0, context.ReadValueAsButton());
        }

        public void OnSpecialAttackP2(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            DoSpecialAttack(1, context.ReadValueAsButton());
        }

        private void DoMove(int pairingIndex, Vector2 move)
        {
            if (IsValidPairing(pairingIndex) == false)
            {
                return;
            }

            if (playerInput.devices[0] == Keyboard.current)
            {
                // Input map normalizes automatically for some reason, un-normalize here
                if (move.x != 0)
                {
                    move.x = Mathf.Sign(move.x);
                }

                if (move.y != 0)
                {
                    move.y = Mathf.Sign(move.y);
                }
            }

            int playerId = PairingIndexToPlayerId(pairingIndex);
            playerInputSo.MoveEvent(playerId)?.Invoke(move);
        }

        public void OnMoveP1(InputAction.CallbackContext context)
        {
            DoMove(0, context.ReadValue<Vector2>());
        }

        public void OnMoveP2(InputAction.CallbackContext context)
        {
            DoMove(1, context.ReadValue<Vector2>());
        }

        private void DoJump(int pairingIndex, bool value)
        {
            if (IsValidPairing(pairingIndex) == false)
            {
                return;
            }

            int playerId = PairingIndexToPlayerId(pairingIndex);
            playerInputSo.JumpEvent(playerId)?.Invoke(value);
        }

        public void OnJumpP1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryAddNewPlayer(0);
            }
            else
            {
                DoJump(0, context.ReadValueAsButton());
            }
        }

        public void OnJumpP2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryAddNewPlayer(1);
            }
            else
            {
                DoJump(1, context.ReadValueAsButton());
            }
        }

        private void DoDash(int pairingIndex, bool val)
        {
            if (IsValidPairing(pairingIndex) == false)
            {
                return;
            }

            int playerId = PairingIndexToPlayerId(pairingIndex);
            playerInputSo.DashEvent(playerId)?.Invoke(val);
        }

        public void OnDashP1(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            DoDash(0, context.ReadValueAsButton());
        }

        public void OnDashP2(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            DoDash(1, context.ReadValueAsButton());
        }
    }
}