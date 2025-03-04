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

        public virtual bool IsInvincible
        {
            get => isInvincible;
            set
            {
                isInvincible = value;
                InvincibleChanged.Invoke(value);
            }
        }

        public UnityAction<bool> InvincibleChanged;

        [ShowNonSerializedField]
        private int entityID;

        private bool isInvincible;

        public virtual void Init(int id)
        {
            EntityID = id;
        }

        public abstract void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact);
    }
}