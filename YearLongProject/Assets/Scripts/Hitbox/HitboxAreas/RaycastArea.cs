using System;
using Hitbox.DataStructures;
using UnityEngine;

namespace Hitbox.HitboxAreas
{
    /// <summary>
    ///     Area for a raycast hitbox
    /// </summary>
    [Serializable]
    public class RaycastArea : IHitboxArea
    {
        /// <summary>
        ///     Rotation of the raycast direction in degrees. 0 goes right
        /// </summary>
        [SerializeField]
        private float rotation;

        /// <summary>
        ///     Offset from the source position to start the raycast
        /// </summary>
        [SerializeField]
        private Vector2 startOffset;

        /// <summary>
        ///     Length of the raycast
        /// </summary>
        [SerializeField]
        private float length;

        public RaycastArea(Vector2 startOffset, float rotation, float length, bool stopOnHit = false)
        {
            this.startOffset = startOffset;
            this.rotation = rotation;
            this.length = length;
            StopOnFirstHit = stopOnHit;
        }

        public bool StopOnFirstHit { get; }

        public Collider2D[] GetCollidersInArea(HitboxContext context)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(
                CalcStartPosition(context),
                CalcDirection(context),
                length,
                context.LayerMask);

            // Sort by distance, since RaycastAll's order isn't defined
            Array.Sort(hit, (a, b) => a.distance.CompareTo(b.distance));

            var colliders = new Collider2D[hit.Length];

            for (var i = 0; i < hit.Length; i++)
            {
                colliders[i] = hit[i].collider;
            }

            return colliders;
        }

        public void DrawAreaDebug(HitboxContext context, DrawDebugConfig config)
        {
            Debug.DrawRay(CalcStartPosition(context),
                CalcDirection(context) * length,
                config.Color,
                config.Duration);
        }

        /// <summary>
        ///     Start position of the raycast
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Vector2 CalcStartPosition(HitboxContext context)
        {
            Vector2 offset = Quaternion.Euler(0, 0, context.SourceAngle) * startOffset;
            if (context.FlipX)
            {
                offset.x = -offset.x;
            }

            return context.SourcePosition + offset;
        }

        private Vector2 CalcDirection(HitboxContext context)
        {
            float angle = context.SourceAngle + rotation;
            if (context.FlipX)
            {
                angle = 180 - angle;
            }

            return Quaternion.Euler(0, 0, angle) * Vector2.right;
        }
    }
}