using Animancer;
using Base;
using Hitbox.DataStructures;
using Hitbox.System;
using UnityEngine;

namespace CharacterScripts
{
    /// <summary>
    ///     Script that handles all hit stop logic
    /// </summary>
    public class HitStop : DescriptionMono
    {
        [Header("Depends")]

        [SerializeField]
        private AnimancerComponent animancerComponent;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Rigidbody2D rb;

        private float hitStopTimer;
        private Vector2 lastVelocity;
        private RigidbodyConstraints2D lastConstraints;

        private void Update()
        {
            if (hitStopTimer > 0)
            {
                hitStopTimer -= Time.deltaTime;
                if (hitStopTimer <= 0)
                {
                    StopHitStop();
                }
            }
        }

        /// <summary>
        ///     Do hit stop when landing a hit
        /// </summary>
        /// <param name="hitboxInstantiateResult"></param>
        public void DoHitStopLandingHit(HitboxInstantiateResult hitboxInstantiateResult)
        {
            if (!hitboxInstantiateResult.HitboxInstance.HitboxEffect.GiveAttackerHitStop)
            {
                return;
            }

            hitStopTimer = hitboxInstantiateResult.HitboxInstance.HitboxEffect.HitPauseDuration;

            if (hitStopTimer > 0)
            {
                StartHitstop();
            }
        }

        /// <summary>
        ///     Do hit stop when hit by something else
        /// </summary>
        /// <param name="hitboxInstance"></param>
        /// <param name="hitImpact"></param>
        public void DoHitStopWhenHit(HitboxInstance hitboxInstance, HitImpact hitImpact)
        {
            if (!hitboxInstance.HitboxEffect.GiveTargetHitStop)
            {
                return;
            }

            hitStopTimer = hitboxInstance.HitboxEffect.HitPauseDuration;

            if (hitStopTimer > 0)
            {
                StartHitstop();
            }
        }

        private void StartHitstop()
        {
            if (animancerComponent)
            {
                animancerComponent.Graph.Speed = 0;
            }

            if (animator)
            {
                animator.speed = 0;
            }

            if (rb)
            {
                lastVelocity = rb.linearVelocity;
                lastConstraints = rb.constraints;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        private void StopHitStop()
        {
            if (animancerComponent)
            {
                animancerComponent.Graph.Speed = 1;
            }

            if (animator)
            {
                animator.speed = 1;
            }

            if (rb)
            {
                rb.constraints = lastConstraints;
                rb.linearVelocity = lastVelocity;
            }
        }
    }
}