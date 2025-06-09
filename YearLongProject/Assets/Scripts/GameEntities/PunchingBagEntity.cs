using Hitbox.DataStructures;
using Hitbox.System;
using Movement;
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
        private CharacterRigidbody2D rb;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public UnityEvent<HitboxInstance, HitImpact> OnHitEvent;

        public override void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact)
        {
            int dir = hitboxInstance.Context.FlipX ? -1 : 1;

            spriteRenderer.transform.localScale = new Vector3(dir, 1, 1);

            Vector2 kb = hitboxInstance.HitboxEffect.Knockback;
            kb.x *= dir;
            rb.LinearVelocity = kb;

            OnHitEvent.Invoke(hitboxInstance, hitImpact);
        }
    }
}