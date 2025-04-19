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
        [BoldHeader("State Cooldown")]
        [InfoBox("Adds a cooldown lockout to a state")]
        [Header("Depends")]

        [SerializeField]
        private CharacterState state;

        [Header("Config")]

        [SerializeField]
        public float staticCooldown;

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
        private float currentCooldown;

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
            cooldownTimer = staticCooldown;
            currentCooldown = staticCooldown;
        }
    }
}