using System;
using System.Collections.Generic;
using Base;
using GameEntities;
using Hitbox.DataStructures;
using Hitbox.HitboxAreas;
using Hitbox.System;
using UnityEngine;
using UnityEngine.Events;

namespace Hitbox.Emitters
{
    /// <summary>
    ///     Emits hitboxes
    /// </summary>
    public class HitboxEmitter : DescriptionMono
    {
        [Serializable]
        public class HitboxLandEvent : UnityEvent<HitboxInstantiateResult>
        {
        }

        private struct HitboxGroupContext
        {
            /// <summary>
            ///     Entities that have been hit by this hitbox group
            /// </summary>
            public List<Entity> HitEntities;
        }

        [Header("Depends")]

        [SerializeField]
        private HitboxSystemSo hitboxSystemSo;

        [SerializeField]
        private Entity entity;

        [SerializeField]
        private Transform hitboxSourceTransform;

        [SerializeField]
        private bool flipX;

        [SerializeField]
        private LayerMask hitboxLayerMask;

        public HitboxLandEvent OnLandHit;

        public Entity Entity => entity;

        /// <summary>
        ///     Maps hitbox group IDs to lists of entities that were hit by them
        ///     This is to prevent multiple hits occuring on the same Entity by one instance of the hitbox group
        /// </summary>
        private readonly Dictionary<string, HitboxGroupContext> hitEntities = new();

        public void EmitHitbox(IHitboxArea hitboxArea, HitboxEffect hitboxEffect, string hitboxGroupId)
        {
            // Create a new hitbox group context if it doesn't exist
            if (!hitEntities.ContainsKey(hitboxGroupId))
            {
                hitEntities.Add(hitboxGroupId, new HitboxGroupContext
                {
                    HitEntities = new List<Entity>()
                });
            }

            HitboxContext context = GetContext(hitboxGroupId);

            var hitboxInstance = new HitboxInstance
            {
                HitboxArea = hitboxArea,
                Context = context,
                HitboxEffect = hitboxEffect
            };

            HitboxInstantiateResult instantiateResult = hitboxSystemSo.InstantiateHitbox(
                hitboxInstance
            );

            if (instantiateResult.HitImpacts.Count > 0)
            {
                OnLandHit?.Invoke(instantiateResult);
                // Add hit entities to hitbox group context
                hitEntities[hitboxGroupId].HitEntities
                    .AddRange(instantiateResult.HitImpacts.ConvertAll(hitImpact => hitImpact.HitEntity));
            }
        }

        public HitboxContext GetContext(string hitboxGroupId)
        {
            bool hitboxGroupContext = hitEntities.TryGetValue(hitboxGroupId, out HitboxGroupContext context);

            return new HitboxContext
            {
                Source = entity,
                SourcePosition = hitboxSourceTransform.position,
                SourceAngle = hitboxSourceTransform.eulerAngles.z,
                LayerMask = hitboxLayerMask,
                FlipX = flipX,
                IgnoreEntities = hitboxGroupContext ? context.HitEntities : new List<Entity>()
            };
        }

        /// <summary>
        ///     Clean out context associated with a hitbox group instance
        /// </summary>
        /// <param name="hitboxGroupId"></param>
        public void EndHitboxGroup(string hitboxGroupId)
        {
            hitEntities.Remove(hitboxGroupId);
        }

        public void SetFlipX(bool flipX)
        {
            this.flipX = flipX;
        }
    }
}