using Hitbox.DataStructures;
using Hitbox.System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEntities
{
    /// <summary>
    ///     A simple entity that just emits events on hit
    /// </summary>
    public class PunchingBagEntity : Entity
    {
        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public UnityEvent<HitboxInstance, HitImpact> OnHitEvent;

        public override void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact)
        {
            int dir = hitboxInstance.Context.FlipX ? -1 : 1;

            spriteRenderer.flipX = dir < 0;

            Vector2 kb = hitboxInstance.HitboxEffect.Knockback;
            kb.x *= dir;
            rb.linearVelocity = kb;

            OnHitEvent.Invoke(hitboxInstance, hitImpact);
        }
    }
}