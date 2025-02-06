using System.Collections.Generic;
using GameEntities;
using Hitbox.DataStructures;
using Hitbox.HitboxAreas;
using UnityEngine;

namespace Hitbox.Emitters
{
    /// <summary>
    ///     Emits hitboxes
    /// </summary>
    public class HitboxEmitter : MonoBehaviour
    {
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

            HitboxSystemSo.HitboxInstantiateResult instantiateResult = hitboxSystemSo.InstantiateHitbox(
                new HitboxInstance
                {
                    HitboxArea = hitboxArea,
                    Context = context,
                    HitboxEffect = hitboxEffect
                });

            // Add hit entities to hitbox group context
            hitEntities[hitboxGroupId].HitEntities.AddRange(instantiateResult.HitEntities);
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