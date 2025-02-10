using Hitbox.DataStructures;
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

        public UnityEvent<bool> OnHitEvent;

        public override void OnHitByAttack(HitboxInstance hitboxInstance)
        {
            Vector2 kb = hitboxInstance.HitboxEffect.Knockback;
            int dir = hitboxInstance.Context.FlipX ? -1 : 1;
            kb.x *= dir;
            rb.linearVelocity = kb;

            OnHitEvent.Invoke(hitboxInstance.Context.FlipX);
        }
    }
}