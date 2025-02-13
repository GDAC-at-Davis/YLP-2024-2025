using Hitbox.HitboxAreas;

namespace Hitbox.DataStructures
{
    /// <summary>
    ///     Data structure containing information for a specific instance of a hitbox
    /// </summary>
    public class HitboxInstance
    {
        /// <summary>
        ///     The untransformed area covered by the hitbox
        ///     This is generally what is configured in the editor
        /// </summary>
        public IHitboxArea HitboxArea;

        /// <summary>
        ///     Context for the hitbox.
        ///     Generally info that might change between different instances of the same hitbox setup, e.g source position
        /// </summary>
        public HitboxContext Context;

        /// <summary>
        ///     Effect that occurs when the hitbox hits a target
        /// </summary>
        public HitboxEffect HitboxEffect;
    }
}