using System;
using System.Collections.Generic;
using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace State_Machine_Scripts.Modifiers
{
    /// <summary>
    ///     Handles switching between states for a simple timing-based combo system
    /// </summary>
    public class ComboStateSwitcher : MonoBehaviour
    {
        [Serializable]
        public struct ComboEntry
        {
            public CharacterState State;

            [Tooltip("Time from starting this state in which the combo is maintained")]
            public float ComboChainTime;
        }

        [BoldHeader("Combo State Switcher")]
        [InfoBox("Handles switching between states for a simple timing-based combo system")]
        [Header("Depends")]

        [SerializeField]
        private List<ComboEntry> comboEntries;

        [SerializeField]
        private CharacterActionManager actionManager;

        [ShowNonSerializedField]
        private int currentComboIndex;

        [ShowNonSerializedField]
        private float comboChainTimer;

        private CharacterState lastQueuedState;
        private float lastQueuedStateTime;

        private void FixedUpdate()
        {
            if (comboChainTimer > 0)
            {
                comboChainTimer -= Time.fixedDeltaTime;

                // Reset the combo
                if (comboChainTimer <= 0)
                {
                    currentComboIndex = 0;
                }
            }
        }

        private void OnDestroy()
        {
            if (lastQueuedState != null)
            {
                lastQueuedState.OnStateEntered.RemoveListener(IncrementState);
            }
        }

        public void TryCombo()
        {
            if (actionManager == null)
            {
                Debug.LogError("Action Manager is not set for combo state switcher", gameObject);
                return;
            }

            // Combos loop
            if (currentComboIndex >= comboEntries.Count)
            {
                currentComboIndex = 0;
            }

            if (lastQueuedState)
            {
                lastQueuedState.OnStateEntered.RemoveListener(IncrementState);
            }

            ComboEntry currentCombo = comboEntries[currentComboIndex];
            currentCombo.State.OnStateEntered.AddListener(IncrementState);
            actionManager.SetState(currentCombo.State);
            lastQueuedState = currentCombo.State;
            lastQueuedStateTime = currentCombo.ComboChainTime;
        }

        private void IncrementState()
        {
            comboChainTimer = lastQueuedStateTime;
            currentComboIndex++;
        }
    }
}