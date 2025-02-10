using System;
using Base;
using UnityEngine;
using UnityEngine.Events;

namespace Input_Scripts
{
    /// <summary>
    ///     Interface exposing input actions for a character instance
    /// </summary>
    public class CharacterActionInput : DescriptionMono
    {
        [Header("Depends")]

        [SerializeField]
        private PlayerInputSo playerInputSo;

        [Header("Input Events")]

        public UnityEvent OnJumpPressed;

        public UnityEvent OnLightAttackPressed;

        // Properties
        public Vector2 MoveInput => moveInput;
        public bool JumpHeld => jumpInputActive;

        // Non-serialized events
        public event Action<Vector2> MoveInputChanged;

        private int playerId;
        private Vector2 moveInput;
        private bool jumpInputActive;

        public void Initialize(int playerId)
        {
            this.playerId = playerId;

            PlayerInputSo.PlayerInputEvents inputs = playerInputSo.TryGetPlayerInputEvents(playerId);

            inputs.MoveEvent += HandleOnMove;
            inputs.DashEvent += HandleOnDash;
            inputs.JumpEvent += HandleOnJump;
            inputs.LightAttackEvent += HandleOnLightAttack;
            inputs.HeavyAttackEvent += HandleOnHeavyAttack;
            inputs.SpecialAttackEvent += HandleOnSpecialAttack;
        }

        public void Cleanup()
        {
            PlayerInputSo.PlayerInputEvents inputs = playerInputSo.TryGetPlayerInputEvents(playerId);

            inputs.MoveEvent -= HandleOnMove;
            inputs.DashEvent -= HandleOnDash;
            inputs.JumpEvent -= HandleOnJump;
            inputs.LightAttackEvent -= HandleOnLightAttack;
            inputs.HeavyAttackEvent -= HandleOnHeavyAttack;
            inputs.SpecialAttackEvent -= HandleOnSpecialAttack;
        }

        private void HandleOnMove(Vector2 move)
        {
            if (move != moveInput)
            {
                MoveInputChanged?.Invoke(move);
            }

            moveInput = move;
        }

        private void HandleOnSpecialAttack(bool arg0)
        {
        }

        private void HandleOnHeavyAttack(bool arg0)
        {
        }

        private void HandleOnLightAttack(bool arg0)
        {
            OnLightAttackPressed.Invoke();
        }

        private void HandleOnJump(bool value)
        {
            if (value)
            {
                OnJumpPressed.Invoke();
            }

            jumpInputActive = value;
        }

        private void HandleOnDash(bool state)
        {
        }
    }
}