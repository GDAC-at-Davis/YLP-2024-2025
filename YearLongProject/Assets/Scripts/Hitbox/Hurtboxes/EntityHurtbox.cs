using Animancer;
using Base;
using GameEntities;
using Hitbox.DataStructures;
using Hitbox.Hurtboxes;
using Hitbox.System;
using UnityEngine;

namespace Hitbox
{
    /// <summary>
    ///     Represents a character hurtbox. Needs to be on the same gameobject as the collider
    /// </summary>
    public class EntityHurtbox : DescriptionMono, IHurtbox
    {
        [SerializeField]
        private Entity attachedEntity;

        public Entity AttachedEntity => attachedEntity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref attachedEntity);
        }
#endif

        public virtual void OnHit(HitboxInstance hitInstance, HitImpact hitImpact)
        {
            if (attachedEntity == null)
            {
                return;
            }

            attachedEntity.OnHitByAttack(hitInstance, hitImpact);
        }
    }
}