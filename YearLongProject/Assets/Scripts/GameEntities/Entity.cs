using Base;
using Hitbox.DataStructures;
using Hitbox.System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEntities
{
    public abstract class Entity : DescriptionMono
    {
        [SerializeField]
        private int entityID;

        public int EntityID
        {
            get => entityID;
            set => entityID = value;
        }

        public UnityAction<bool> InvincibleChanged;
        private bool isInvincible = false;
        public virtual bool IsInvincible 
        {
            get => isInvincible;
            set
            {
                isInvincible = value;
                InvincibleChanged.Invoke(value);
            }
        }
        

        public virtual void Init(int id)
        {
            EntityID = id;
        }

        public abstract void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact);
    }
}