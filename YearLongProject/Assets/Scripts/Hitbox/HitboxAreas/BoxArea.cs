using System;
using Hitbox.DataStructures;
using UnityEngine;

namespace Hitbox.HitboxAreas
{
    /// <summary>
    ///     Simple box area. Can be rotated
    /// </summary>
    [Serializable]
    public class BoxArea : IHitboxArea
    {
        /// <summary>
        ///     Position of the center of the box, in the origin's local space
        /// </summary>
        [SerializeField]
        private Vector2 center;

        /// <summary>
        ///     Rotation of the box, in degrees
        /// </summary>
        [SerializeField]
        private float rotation;

        /// <summary>
        ///     Size of the box
        /// </summary>
        [SerializeField]
        private Vector2 size;

        public BoxArea(Vector2 center, float rotation, Vector2 size)
        {
            this.center = center;
            this.rotation = rotation;
            this.size = size;
        }

        /// <summary>
        ///     No way to get the first hit with overlap boxes, so irrelevant
        /// </summary>
        public bool StopOnFirstHit => false;

        public Collider2D[] GetCollidersInArea(HitboxContext context)
        {
            TransformToContext(context, out Vector2 center, out float rotation);

            Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, rotation, context.LayerMask);

            return hits;
        }

        public void DrawAreaDebug(HitboxContext context, DrawDebugConfig debug)
        {
            GetCornerPositions(context, out Vector2 topLeft, out Vector2 topRight, out Vector2 bottomLeft,
                out Vector2 bottomRight);

            Debug.DrawLine(topLeft, topRight, debug.Color, debug.Duration);
            Debug.DrawLine(topRight, bottomRight, debug.Color, debug.Duration);
            Debug.DrawLine(bottomRight, bottomLeft, debug.Color, debug.Duration);
            Debug.DrawLine(bottomLeft, topLeft, debug.Color, debug.Duration);
        }

        private void TransformToContext(HitboxContext context, out Vector2 pos, out float rotation)
        {
            // Transform the center of the box to world space
            pos = context.SourcePosition + (Vector2)(Quaternion.Euler(0, 0, context.SourceAngle) * center);

            // Transform rotation to world space
            rotation = context.SourceAngle + this.rotation;

            if (context.FlipX)
            {
                pos.x = context.SourcePosition.x - (pos.x - context.SourcePosition.x);
                rotation = 180 - rotation;
            }
        }

        public void GetCornerPositions(HitboxContext context, out Vector2 topLeft, out Vector2 topRight,
            out Vector2 bottomLeft, out Vector2 bottomRight)
        {
            TransformToContext(context, out Vector2 center, out float rotation);

            float halfWidth = size.x / 2;
            float halfHeight = size.y / 2;

            Quaternion rotationQuaternion = Quaternion.Euler(0, 0, rotation);
            topLeft = center + (Vector2)(rotationQuaternion * new Vector2(-halfWidth, halfHeight));
            topRight = center + (Vector2)(rotationQuaternion * new Vector2(halfWidth, halfHeight));
            bottomLeft = center + (Vector2)(rotationQuaternion * new Vector2(-halfWidth, -halfHeight));
            bottomRight = center + (Vector2)(rotationQuaternion * new Vector2(halfWidth, -halfHeight));
        }
    }
}