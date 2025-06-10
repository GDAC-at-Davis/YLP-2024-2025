using Hitbox.DataStructures;
using Hitbox.System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace GameEntities
{
    public abstract class Entity : MonoBehaviour
    {
        public int EntityID
        {
            get => entityID;
            set => entityID = value;
        }

        public virtual bool IsInvincible => invincibleLockCounter > 0;

        public UnityAction<bool> InvincibleChanged;

        [ShowNonSerializedField]
        private int entityID;

        private int invincibleLockCounter;

        public void AddInvincibility()
        {
            invincibleLockCounter++;
            if (invincibleLockCounter == 1)
            {
                InvincibleChanged?.Invoke(true);
            }
        }

        public void RemoveInvincibility()
        {
            invincibleLockCounter--;
            if (invincibleLockCounter == 0)
            {
                InvincibleChanged?.Invoke(false);
            }

            if (invincibleLockCounter < 0)
            {
                invincibleLockCounter = 0;
            }
        }

        public virtual void Init(int id)
        {
            EntityID = id;
        }

        public abstract void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact);
    }
}