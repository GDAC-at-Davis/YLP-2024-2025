using System;
using System.Collections.Generic;
using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace State_Machine_Scripts.Modifiers
{
    /// <summary>
    ///     Applies cooldown to a state
    /// </summary>
    public class StateCooldown : MonoBehaviour
    {
        /// <summary>
        ///     A tier of cooldown, for scaling cooldown
        /// </summary>
        [Serializable]
        public struct CooldownTier
        {
            public float CooldownTime;
        }

        [BoldHeader("State Cooldown")]
        [InfoBox("Adds a cooldown lockout to a state")]
        [Header("Depends")]

        [SerializeField]
        private CharacterState state;

        [Header("Config")]

        [SerializeField]
        private float lockoutDuration;

        [SerializeField]
        public List<CooldownTier> cooldownTiers;

        /// <summary>
        ///     How much timer is left on the cooldown
        /// </summary>
        [Header("Debug")]

        public float CooldownTimer => cooldownTimer;

        /// <summary>
        ///     How long is the cooldown that is currently ticking down
        /// </summary>
        public float CurrentCooldown => currentCooldown;

        [ShowNonSerializedField]
        private float cooldownTimer;

        [ShowNonSerializedField]
        private float timeSinceOffCooldown;

        [ShowNonSerializedField]
        private float currentCooldown;

        [ShowNonSerializedField]
        private int cooldownTier;

        private void Update()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;

                if (cooldownTimer <= 0)
                {
                    state.RemoveLockout();
                    cooldownTimer = 0;
                }
            }
            else
            {
                timeSinceOffCooldown += Time.deltaTime;
            }
        }

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
            state.AddLockout();

            // Used too early, increase tier
            if (timeSinceOffCooldown <= lockoutDuration)
            {
                cooldownTier = Mathf.Clamp(cooldownTier + 1, 0, cooldownTiers.Count - 1);
            }
            else
            {
                cooldownTier = 0;
            }

            timeSinceOffCooldown = 0;
            currentCooldown = cooldownTiers[cooldownTier].CooldownTime;
            cooldownTimer = currentCooldown;

            if (cooldownTimer == 0)
            {
                cooldownTimer = Time.fixedDeltaTime;
            }
        }
    }
}