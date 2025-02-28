using Animancer;
using Hitbox.DataStructures;
using Hitbox.System;
using Input_Scripts;
using State_Machine_Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GameEntities
{
    public class CharacterEntity : Entity
    {
        [Header("Depends")]

        [SerializeField]
        public CharacterActionManager ActionManager;

        [SerializeField]
        private CharacterActionInput actionInput;

        [SerializeField]
        private SimpleMovementController movementController;

        public UnityEvent<HitboxInstance, HitImpact> OnHitByAttackEvent;

        [SerializeField]
        private int health = 50;

        public bool Initialized => playerId != -1;

        /// <summary>
        ///     Id of the actual player. Used for input and other player specific things.
        /// </summary>
        public int PlayerId => playerId;

        public int Health => health;
        public UnityAction<int> UpdateHealth;

        private int playerId = -1;

        public void OnDestroy()
        {
            actionInput.Cleanup();
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
            ActionManager.Initialize(actionInput);
        }

        // Callback for this Character being hit by an attack
        // Will route calls to health/stats manager, action manager
        // Example of override: reflecting damage back at attacker
        public override void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact)
        {
            // TODO: move this logic into a function in movement controller?
            Vector2 knockback = hitboxInstance.HitboxEffect.Knockback;
            knockback = new Vector2(knockback.x * (hitboxInstance.Context.FlipX ? -1 : 1), knockback.y);
            movementController.Knockback = knockback;
            movementController.stunTime = Time.time + hitboxInstance.HitboxEffect.Hitstun;

            TakeDamage((int)hitboxInstance.HitboxEffect.Damage);

            ActionManager.SetState("AhabHitstun");

            OnHitByAttackEvent?.Invoke(hitboxInstance, hitImpact);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            UpdateHealth.Invoke(health);

            if (health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            //Temporary implementation
            Destroy(gameObject);
        }
    }
}