using Animancer;
using Hitbox.DataStructures;
using Input_Scripts;
using State_Machine_Scripts;
using UnityEngine;

namespace GameEntities
{
    public class Character : Entity
    {
        [Header("Depends")]

        [SerializeField]
        public CharacterActionManager ActionManager;

        [SerializeField]
        private CharacterActionInput actionInput;

        /// <summary>
        ///     Id of the actual player. Used for input and other player specific things.
        /// </summary>
        public int PlayerId => playerId;

        private int playerId = -1;

        protected bool IsInvincible;

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
        public override void OnHitByAttack(HitboxInstance hitboxInstance)
        {
            if (IsInvincible)
            {
            }
        }

        // Callback for landing an attack on a Character
        // Example of override: granting this Character buffs on landing hit
        public virtual void OnAttackHit(Character other)
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