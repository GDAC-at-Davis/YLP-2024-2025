using Animancer;
using EditorUtils.BoldHeader;
using Hitbox.DataStructures;
using Hitbox.System;
using Input_Scripts;
using Managers;
using Movement;
using NaughtyAttributes;
using State_Machine_Scripts;
using State_Machine_Scripts.States;
using UnityEngine;
using UnityEngine.Events;
using UnityEvent = UnityEngine.Events.UnityEvent;

namespace GameEntities
{
    public class CharacterEntity : Entity
    {
        [BoldHeader("Character Entity Script")]
        [InfoBox("The top-level script representing a character entity. Don't remove!")]
        [Header("Dependencies")]

        [SerializeField]
        private GameDataSO gameDataSO;

        [SerializeField]
        public CharacterActionManager ActionManager;

        [SerializeField]
        private CharacterActionInput actionInput;

        [SerializeField]
        private SimpleMovementController movementController;
        public SimpleMovementController MovementController => movementController;

        [SerializeField]
        private HitstunState hitstunState;

        // TODO: Temp reset
        [SerializeField]
        [Scene]
        private string endSceneName;

        [Header("Events")]

        [InfoBox(
            "Add listeners to these UnityEvents to define custom behavior when the character is hit by an attack.")]
        public UnityEvent<HitboxInstance, HitImpact> OnHitByAttackEvent;

        public UnityEvent OnDefeated;

        [SerializeField]
        private int health = 50;

        public bool Initialized => playerId != -1;

        /// <summary>
        ///     Id of the actual player. Used for input and other player specific things.
        /// </summary>
        public int PlayerId => playerId;

        public int Health => health;

        public float StunTime => stunTime;
        public int MaxHealth { get; private set; }

        public Color PlayerColor
        {
            get
            {
                if (playerId == -1)
                {
                    Debug.Log("PlayerId is not set. Returning default color.");
                    return Color.white;
                }

                return gameDataSO.PlayerColors[playerId];
            }
        }

        public UnityAction<int, int> UpdateHealth;

        private int playerId = -1;

        private float stunTime;

        private void Awake()
        {
            // Initialize the action manager in Awake, so we don't need input yet
            // This is useful for the quick testing scene for taking damage without an extra input device
            ActionManager.Initialize(actionInput);
        }

        public void OnDestroy()
        {
            actionInput.Cleanup();
            gameDataSO.SetCharacterEntity(playerId, null);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref ActionManager);
        }
#endif

        public void Initialize(int id)
        {
            playerId = id;
            transform.parent = null;

            actionInput.Initialize(id);
            MaxHealth = health;

            gameDataSO.SetCharacterEntity(playerId, this);
        }

        // Callback for this Character being hit by an attack
        // Will route calls to health/stats manager, action manager
        // Example of override: reflecting damage back at attacker
        public override void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact)
        {
            // TODO: move this logic into a function in movement controller?
            Vector2 knockback = hitboxInstance.CalculatedKnockback();
            stunTime = Time.time + hitboxInstance.HitboxEffect.Hitstun;

            hitstunState.SetKnockback(knockback);

            TakeDamage((int)hitboxInstance.HitboxEffect.Damage);

            OnHitByAttackEvent?.Invoke(hitboxInstance, hitImpact);
        }

        public void TakeDamage(int damage)
        {
            if (health < 0)
            {
                return;
            }

            health -= damage;
            UpdateHealth?.Invoke(playerId, health);

            if (health <= 0)
            {
                Die();
            }
        }

        // Callback for landing an attack on a Character
        // Example of override: granting this Character buffs on landing hit
        public virtual void OnAttackHit(HitboxInstantiateResult result)
        {
        }

        public virtual void SetIsInvincible(bool isInvincible)
        {
            IsInvincible = isInvincible;
        }

        public virtual void Die()
        {
            OnDefeated?.Invoke();
        }
    }
}