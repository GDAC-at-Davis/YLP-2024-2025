using UnityEngine;

namespace Hitbox.HitboxAreas
{
    /// <summary>
    ///     Interface representing a hitbox's area
    /// </summary>
    public interface IHitboxArea
    {
        /// <summary>
        ///     Run physics check for colliders in the area
        /// </summary>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public Collider2D[] GetCollidersInArea(HitboxCheckContext layerMask);

        /// <summary>
        ///     Visualize the area for debugging purposes
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public void DrawAreaDebug(HitboxCheckContext context, DrawDebugConfig config);
    }

    /// <summary>
    ///     Structure with context for performing a HitboxArea check
    /// </summary>
    public struct HitboxCheckContext
    {
        /// <summary>
        ///     Layer mask to check for
        /// </summary>
        public LayerMask LayerMask;

        /// <summary>
        ///     Position of the "origin" for the hitbox area.
        ///     Generally related to the entity performing the action this hitbox is for.
        /// </summary>
        public Vector2 SourcePosition;

        /// <summary>
        ///     Rotation of the "origin" for the hitbox area.
        /// </summary>
        public float SourceAngle;

        /// <summary>
        ///     Direction the entity is flipped to face. -1 for left, 1 for right.
        /// </summary>
        public int FacingDirection;
    }

    /// <summary>
    ///     Configuration for drawing debug lines
    /// </summary>
    public struct DrawDebugConfig
    {
        public Color Color;
        public float Duration;

        public DrawDebugConfig(Color color, float duration)
        {
            Color = color;
            Duration = duration;
        }
    }
}