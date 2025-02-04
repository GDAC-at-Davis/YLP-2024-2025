using System.Collections.Generic;
using Base;
using Hitbox.DataStructures;
using Hitbox.HitboxAreas;
using UnityEngine;

namespace Hitbox
{
    [CreateAssetMenu(menuName = "Systems/HitboxSystem")]
    public class HitboxSystemSO : DescriptionSO
    {
        [SerializeField]
        [Tooltip("Whether to draw hitbox areas for debugging purposes")]
        private bool _showHitboxAreas;

        [SerializeField]
        private bool _detailedLogging;

        [SerializeField]
        private float _hitboxVisualizeDuration;

        /// <summary>
        ///     Instantiates a hitbox and applies its effects
        /// </summary>
        /// <param name="hitboxInstance"></param>
        public void InstantiateHitbox(HitboxInstance hitboxInstance)
        {
            HitboxContext context = hitboxInstance.Context;
            HitboxEffect effect = hitboxInstance.HitboxEffect;
            IHitboxArea area = hitboxInstance.HitboxArea;

            if (_showHitboxAreas)
            {
                area.DrawAreaDebug(context, new DrawDebugConfig(Color.red, _hitboxVisualizeDuration));
            }

            Collider2D[] hits = area.GetCollidersInArea(context);

            // TODO: Are we moving to a more generic 'Entity' type?

            // Prevent a character from being hit multiple times by the same hitbox
            var hitEntities = new HashSet<Entity>();

            foreach (Collider2D hit in hits)
            {
                if (_detailedLogging)
                {
                    Debug.Log($"Hitbox overlapped collider {hit.gameObject}");
                }

                var hurtbox = hit.GetComponent<EntityHurtbox>();

                if (hurtbox == null)
                {
                    continue;
                }

                Entity entity = hurtbox.AttachedEntity;

                if (entity != null && entity == context.SourceEntity)
                {
                    continue;
                }

                if (hitEntities.Contains(entity))
                {
                    continue;
                }

                if (entity != null)
                {
                    hitEntities.Add(entity);
                }

                if (_detailedLogging)
                {
                    Debug.Log($"Hit Hurtbox {hit.gameObject}", hit.gameObject);
                }

                hurtbox.OnHit(hitboxInstance);
            }
        }
    }
}