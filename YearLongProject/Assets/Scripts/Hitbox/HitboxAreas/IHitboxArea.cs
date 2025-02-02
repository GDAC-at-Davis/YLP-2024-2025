using Hitbox.DataStructures;
using UnityEngine;

namespace Hitbox.HitboxAreas
{
    /// <summary>
    ///     Interface representing a hitbox's area.
    ///     This generally is in local space to the entity performing the action.
    /// </summary>
    public interface IHitboxArea
    {
        /// <summary>
        ///     Run physics check for colliders in the area
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Collider2D[] GetCollidersInArea(HitboxContext context);

        /// <summary>
        ///     Visualize the area for debugging purposes
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public void DrawAreaDebug(HitboxContext context, DrawDebugConfig config);
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