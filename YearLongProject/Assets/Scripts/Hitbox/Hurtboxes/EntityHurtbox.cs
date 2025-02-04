using Base;
using Hitbox.DataStructures;
using Hitbox.Hurtboxes;
using UnityEngine;

namespace Hitbox
{
    /// <summary>
    ///     Represents a character hurtbox. Needs to be on the same gameobject as the collider
    /// </summary>
    public class EntityHurtbox : DescriptionMono, IHurtbox
    {
        [SerializeField]
        private Entity _attachedEntity;

        public Entity AttachedEntity => _attachedEntity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _attachedEntity = GetComponentInParent<Entity>();
        }
#endif

        public void OnHit(HitboxInstance hitboxInstance)
        {
            if (_attachedEntity == null)
            {
                return;
            }

            _attachedEntity.OnHitByAttack(hitboxInstance);
        }
    }
}