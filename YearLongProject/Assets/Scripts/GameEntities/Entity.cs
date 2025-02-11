using Base;
using Hitbox.DataStructures;
using UnityEngine;

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

        public virtual void Init(int id)
        {
            EntityID = id;
        }

        public abstract void OnHitByAttack(HitboxInstance hitboxInstance);
    }
}