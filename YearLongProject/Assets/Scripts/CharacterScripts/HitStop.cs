using Base;
using Hitbox.DataStructures;
using Hitbox.System;
using Movement;
using State_Machine_Scripts;
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
        private CharacterActionManager actionManager;

        [SerializeField]
        private CharacterRigidbody2D rb;

        private float hitStopTimer;
        private Vector2 lastVelocity;

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
            HitboxEffect hitboxEffect = hitboxInstantiateResult.HitboxInstance.HitboxEffect;
            if (!hitboxEffect.GiveAttackerHitStop)
            {
                return;
            }

            float hitStopDuration = hitboxEffect.HitStopDuration;

            if (hitStopDuration > 0)
            {
                hitStopTimer = hitStopDuration;
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
            HitboxEffect hitboxEffect = hitboxInstance.HitboxEffect;

            if (!hitboxEffect.GiveTargetHitStop)
            {
                return;
            }

            float hitStopDuration = hitboxEffect.HitStopDuration;

            if (hitStopDuration > 0)
            {
                hitStopTimer = hitStopDuration;
                StartHitstop();
            }
        }

        private void StartHitstop()
        {
            if (actionManager)
            {
                actionManager.InternalFixedTimeScale = 0;
            }

            if (rb)
            {
                rb.SetFrozen(true);
            }
        }

        private void StopHitStop()
        {
            if (actionManager)
            {
                actionManager.InternalFixedTimeScale = 1;
            }

            if (rb)
            {
                rb.SetFrozen(false);
            }
        }
    }
}