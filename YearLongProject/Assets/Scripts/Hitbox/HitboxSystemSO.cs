using System.Collections.Generic;
using Base;
using GameEntities;
using Hitbox.DataStructures;
using Hitbox.HitboxAreas;
using UnityEngine;

namespace Hitbox
{
    [CreateAssetMenu(menuName = "Systems/HitboxSystem")]
    public class HitboxSystemSo : DescriptionSO
    {
        public struct HitboxInstantiateResult
        {
            public List<Entity> HitEntities;
        }

        [SerializeField]
        [Tooltip("Whether to draw hitbox areas for debugging purposes")]
        private bool showHitboxAreas;

        [SerializeField]
        private bool detailedLogging;

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

            if (showHitboxAreas)
            {
                area.DrawAreaDebug(context, new DrawDebugConfig(Color.red, hitboxVisualizeDuration));
            }

            Collider2D[] hits = area.GetCollidersInArea(context);

            var hitList = new List<Entity>();

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

                if (hitList.Contains(entity))
                {
                    continue;
                }

                if (detailedLogging)
                {
                    Debug.Log($"Hit Hurtbox {hit.gameObject}", hit.gameObject);
                }

                hurtbox.OnHit(hitboxInstance);

                hitList.Add(entity);

                if (area.StopOnFirstHit)
                {
                    break;
                }
            }

            return new HitboxInstantiateResult
            {
                HitEntities = hitList
            };
        }
    }
}