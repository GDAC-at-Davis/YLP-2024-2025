using System;
using System.Collections.Generic;
using EditorUtils.BoldHeader;
using Movement;
using NaughtyAttributes;
using State_Machine_Scripts.States;
using UnityEngine;

namespace State_Machine_Scripts.Modifiers
{
    /// <summary>
    ///     Allows a character to move during a move, but only when grounded
    /// </summary>
    public class GroundedMovementLock : MonoBehaviour
    {
        [BoldHeader("Grounded State")]
        [InfoBox("Disable movement if character is grounded during state")]
        [Header("Depends")]

        [SerializeField]
        private SimpleTimelineState state;

        [Header("Dependencies")]
        [SerializeField]
        private SimpleMovementController movementController;

        [Header("Config")]

        [SerializeField]
        private bool invert;

        private void OnEnable()
        {
            state.OnStateEntered.AddListener(HandleOnStateEntered);
        }

        private void OnDisable()
        {
            state.OnStateEntered.RemoveListener(HandleOnStateEntered);
        }

        private void HandleOnStateEntered()
        {

            if (movementController.GetIsGrounded())
            {
                state.useDefaultMovement = invert;
            }
            else
            {
                state.useDefaultMovement = !invert;
            }
        }
    }
}