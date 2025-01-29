using UnityEngine;

namespace Hitbox.HitboxAreas
{
    /// <summary>
    ///     Simple box area. Can be rotated
    /// </summary>
    public class BoxArea : IHitboxArea
    {
        /// <summary>
        ///     Position of the center of the box, in the origin's local space
        /// </summary>
        private readonly Vector2 _center;

        /// <summary>
        ///     Rotation of the box, in degrees
        /// </summary>
        private readonly float _rotation;

        /// <summary>
        ///     Size of the box
        /// </summary>
        private readonly Vector2 _size;

        public BoxArea(Vector2 center, float rotation, Vector2 size)
        {
            _center = center;
            _rotation = rotation;
            _size = size;
        }

        public Collider2D[] GetCollidersInArea(HitboxCheckContext context)
        {
            TransformToOrigin(context, out Vector2 center, out float rotation);

            Collider2D[] hits = Physics2D.OverlapBoxAll(center, _size, rotation, context.LayerMask);

            return hits;
        }

        public void DrawAreaDebug(HitboxCheckContext context, DrawDebugConfig debug)
        {
            GetCornerPositions(context, out Vector2 topLeft, out Vector2 topRight, out Vector2 bottomLeft,
                out Vector2 bottomRight);

            Debug.DrawLine(topLeft, topRight, debug.Color, debug.Duration);
            Debug.DrawLine(topRight, bottomRight, debug.Color, debug.Duration);
            Debug.DrawLine(bottomRight, bottomLeft, debug.Color, debug.Duration);
            Debug.DrawLine(bottomLeft, topLeft, debug.Color, debug.Duration);
        }

        private void TransformToOrigin(HitboxCheckContext context, out Vector2 pos, out float rotation)
        {
            // Transform the center of the box to world space
            pos = context.SourcePosition + (Vector2)(Quaternion.Euler(0, 0, context.SourceAngle) * _center);

            // Transform rotation to world space
            rotation = context.SourceAngle + _rotation;
        }

        public void GetCornerPositions(HitboxCheckContext context, out Vector2 topLeft, out Vector2 topRight,
            out Vector2 bottomLeft, out Vector2 bottomRight)
        {
            TransformToOrigin(context, out Vector2 center, out float rotation);

            float halfWidth = _size.x / 2;
            float halfHeight = _size.y / 2;

            Quaternion rotationQuaternion = Quaternion.Euler(0, 0, rotation);
            topLeft = center + (Vector2)(rotationQuaternion * new Vector2(-halfWidth, halfHeight));
            topRight = center + (Vector2)(rotationQuaternion * new Vector2(halfWidth, halfHeight));
            bottomLeft = center + (Vector2)(rotationQuaternion * new Vector2(-halfWidth, -halfHeight));
            bottomRight = center + (Vector2)(rotationQuaternion * new Vector2(halfWidth, -halfHeight));
        }
    }
}