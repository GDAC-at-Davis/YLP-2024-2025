using System;
using System.Collections.Generic;
using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

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

        private readonly List<UnityAction> stateEnteredListeners = new();

        [ShowNonSerializedField]
        private int currentComboIndex;

        [ShowNonSerializedField]
        private float comboChainTimer;

        private CharacterState lastQueuedState;

        private void Start()
        {
            for (var i = 0; i < comboEntries.Count; i++)
            {
                int index = i;
                ComboEntry entry = comboEntries[i];
                var stateEnteredListener = new UnityAction(() => HandleStateEntered(index));
                stateEnteredListeners.Add(stateEnteredListener);
                entry.State.OnStateEntered.AddListener(stateEnteredListener);
            }
        }

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
            if (actionManager == null)
            {
                return;
            }

            for (var i = 0; i < comboEntries.Count; i++)
            {
                int index = i;
                ComboEntry entry = comboEntries[i];
                entry.State.OnStateEntered.RemoveListener(stateEnteredListeners[index]);
            }
        }

        public void TryCombo()
        {
            if (actionManager == null)
            {
                Debug.LogError("Action Manager is not set for combo state switcher", gameObject);
                return;
            }

            ComboEntry currentCombo = comboEntries[currentComboIndex];
            actionManager.SetState(currentCombo.State);
        }

        private void HandleStateEntered(int enteredIndex)
        {
            lastQueuedState = null;
            ComboEntry currentCombo = comboEntries[enteredIndex];
            comboChainTimer = currentCombo.ComboChainTime;

            currentComboIndex = enteredIndex + 1;
            if (currentComboIndex >= comboEntries.Count)
            {
                currentComboIndex = 0;
            }
        }
    }
}