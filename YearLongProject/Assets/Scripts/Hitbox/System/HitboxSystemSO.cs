using System.Collections.Generic;
using Base;
using GameEntities;
using Hitbox.DataStructures;
using Hitbox.HitboxAreas;
using UnityEngine;

namespace Hitbox.System
{
    [CreateAssetMenu(menuName = "Systems/HitboxSystem")]
    public class HitboxSystemSo : DescriptionSO
    {
        [SerializeField]
        [Tooltip("Whether to draw hitbox areas for debugging purposes")]
        public static bool ShowHitboxAreas;

        [SerializeField]
        private float hitboxVisualizeDuration;

        /// <summary>
        ///     Instantiates a hitbox and applies its effects
        /// </summary>
        /// <param name="hitboxInstance"></param>
        public HitboxInstantiateResult InstantiateHitbox(HitboxInstance hitboxInstance)
        {
            HitboxContext context = hitboxInstance.Context;
            HitboxEffect effect = hitboxInstance.HitboxEffect;
            IHitboxArea area = hitboxInstance.HitboxArea;

            if (ShowHitboxAreas)
            {
                area.DrawAreaDebug(context, new DrawDebugConfig(Color.red, hitboxVisualizeDuration));
            }

            Collider2D[] hits = area.GetCollidersInArea(context);

            var hitEntities = new List<Entity>();
            var hitImpacts = new List<HitImpact>();

            foreach (Collider2D hit in hits)
            {
                var hurtbox = hit.GetComponent<EntityHurtbox>();

                if (hurtbox == null)
                {
                    continue;
                }

                Entity entity = hurtbox.AttachedEntity;

                if (entity == null)
                {
                    continue;
                }

                if (entity == context.Source)
                {
                    continue;
                }

                if (context.IgnoreEntities.Contains(entity))
                {
                    continue;
                }

                if (hitEntities.Contains(entity))
                {
                    continue;
                }

#if UNITY_EDITOR
                if (ShowHitboxAreas)
                {
                    Debug.Log($"Hit Hurtbox {hit.gameObject}", hit.gameObject);
                }
#endif

                var hitImpact = new HitImpact
                {
                    HitEntity = entity
                };

                hurtbox.OnHit(hitboxInstance, hitImpact);

                hitEntities.Add(entity);
                hitImpacts.Add(hitImpact);

                if (area.StopOnFirstHit)
                {
                    break;
                }
            }

            return new HitboxInstantiateResult
            {
                HitboxInstance = hitboxInstance,
                HitImpacts = hitImpacts
            };
        }
    }
}